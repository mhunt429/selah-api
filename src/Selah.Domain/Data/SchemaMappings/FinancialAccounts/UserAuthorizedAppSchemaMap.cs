using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Integrations;

namespace Selah.Domain.Data.SchemaMappings.FinancialAccounts
{
    public class UserAuthorizedSchemaMap : EntityMap<UserAuthorizedApp>
    {
        public UserAuthorizedSchemaMap()
        {
            Map(u => u.Id)
              .ToColumn("id");

            Map(u => u.UserId)
              .ToColumn("user_id");

            Map(u => u.Scope)
              .ToColumn("scope");

            Map(u => u.EncryptedAccessToken)
              .ToColumn("encrypted_access_token");

            Map(u => u.EncryptedRefreshToken)
              .ToColumn("encrypted_refresh_token");

            Map(u => u.AccessTokenExpirationTs)
              .ToColumn("access_token_expiration_ts");

            Map(u => u.RefreshTokenExpirationTs)
              .ToColumn("refresh_token_expiration_ts");

            Map(u => u.CreatedTs)
              .ToColumn("created_ts");

            Map(u => u.UpdatedTs)
              .ToColumn("updated_ts");
        }
    }
}