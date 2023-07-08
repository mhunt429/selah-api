using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Amazon.Runtime;
using HashidsNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selah.Application.Services.Interfaces;

namespace Selah.Application.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly string _accessKey;
        private readonly string _kmsKey;
        private readonly ILogger _logger;
        private readonly string _secretKey;
        private readonly RegionEndpoint _region = RegionEndpoint.USEast1;
        private readonly IConfiguration _configuration;
        private readonly Hashids _hashId;

        public SecurityService(ILogger<SecurityService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var awsClientConfig = _configuration.GetSection("AWS_CONFIG");
            _accessKey = awsClientConfig["ACCESS_KEY"];
            _secretKey = awsClientConfig["SECRET"];
            _kmsKey = awsClientConfig["KMS_KEY"];
            _hashId = new Hashids(_configuration["HASH_ID_SALT"], minHashLength: 24);
        }

        public async Task<string> Encrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                var exception = new ArgumentException(nameof(input));
                _logger.LogError(exception, "Can't encrypt an empty value'");
                return "";
            }

            try
            {
                _logger.LogDebug($"Encrypting with {_accessKey} and {_kmsKey}");

                var encryptedString = "";

                var textBytes = Encoding.UTF8.GetBytes(input);
                var stringToStream = new MemoryStream(textBytes, 0, textBytes.Length);

                using (var kmsClient =
                       new AmazonKeyManagementServiceClient(new BasicAWSCredentials(_accessKey, _secretKey),
                           _region))
                {
                    var response = await kmsClient.EncryptAsync(new EncryptRequest
                    {
                        KeyId = _kmsKey,
                        Plaintext = stringToStream
                    });
                    encryptedString = Convert.ToBase64String(response.CiphertextBlob.ToArray());
                }

                _logger.LogDebug($"Message Encrypted {encryptedString}");

                return encryptedString;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"AWS Encryption threw an error:\nSource: {ex.Source}\nMessage: {ex.Message}\nStack:{ex.StackTrace}");
                return null;
            }
        }

        public async Task<string> Decrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                var exception = new ArgumentException(nameof(input));
                _logger.LogError(exception, "Can't decrypt an empty value'");
                return "";
            }

            try
            {
                _logger.LogDebug($"Decrypting with {_accessKey} and {_kmsKey}");

                var decryptedString = "";
                using (var kmsClient =
                       new AmazonKeyManagementServiceClient(
                           new BasicAWSCredentials(_accessKey.Trim(), _secretKey.Trim()),
                           _region))
                {
                    var fromBase64 = Convert.FromBase64String(input);
                    var stringToStream = new MemoryStream(fromBase64, 0, fromBase64.Length);

                    var response = await kmsClient.DecryptAsync(new DecryptRequest
                    {
                        CiphertextBlob = stringToStream
                    });

                    using (var stream = new StreamReader(response.Plaintext))
                    {
                        decryptedString = await stream.ReadToEndAsync();
                    }
                }

                _logger.LogDebug($"Message Decrypted {decryptedString}");

                return decryptedString;
            }
            catch (InvalidCiphertextException ex)
            {
                _logger.LogError(
                    $"AWS Returned an error reverting to legacy decryption. \nType: {ex.GetType()}\nSource: {ex.Source}\nMessage: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"An error occured attempting to decrypt \nType: {ex.GetType()}\nSource: {ex.Source}\nMessage: {ex.Message}\nzStack: {ex.StackTrace}");
                return null;
            }
        }

        public int DecodeHashId(string hashId)
        {
            int[] decodedId = _hashId.Decode((hashId));
            return decodedId.Length > 0 ? decodedId[0] : 0;
        }

        public string EncodeHashId(int id)
        {
            return _hashId.Encode(id);
        }
    }
}