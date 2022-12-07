using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Internal;

namespace Selah.Application.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly string _accessKey;
        private readonly string _kmsKey;
        private readonly ILogger _logger;
        private readonly string _secretKey;
        private readonly RegionEndpoint _region = RegionEndpoint.USEast1;

        public SecurityService(ILogger<SecurityService> logger, IOptions<EnvVariablesConfig> awsClientConfig)
        {
            _logger = logger;
            var awsClientConfig1 = awsClientConfig;
            _accessKey = awsClientConfig1.Value.AwsAccessKey.Trim();
            _secretKey = awsClientConfig1.Value.AwsSecretKey.Trim();
            _kmsKey = awsClientConfig1.Value.KmsEncryptionKeyId.Trim();
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
                    new AmazonKeyManagementServiceClient(new BasicAWSCredentials(_accessKey.Trim(), _secretKey.Trim()),
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
    }
}