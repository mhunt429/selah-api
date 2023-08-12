using System;

namespace Selah.Domain.Internal
{
    public class EnvVariablesConfig
    {
        public string DbConnectionString => Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public string JwtSecret => Environment.GetEnvironmentVariable("JWT_SECRET");
        public string JwtIssuer => Environment.GetEnvironmentVariable("JWT_ISSUER");

        public string AwsAccessKey => Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        public string AwsSecretKey => Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        public string KmsEncryptionKeyId => Environment.GetEnvironmentVariable("KMS_ENCRYPTION_KEY_ID");

        public string PlaidEnv => Environment.GetEnvironmentVariable("PLAID_ENV");
        public string PlaidClientId => Environment.GetEnvironmentVariable("PLAID_CLIENT_ID");
        public string PlaidClientSecret => Environment.GetEnvironmentVariable("PLAID_CLIENT_SECRET");

        public string TDAmeritradeAPIKey => Environment.GetEnvironmentVariable("TD_AMERITRADE_API_KEY");
    }
}
