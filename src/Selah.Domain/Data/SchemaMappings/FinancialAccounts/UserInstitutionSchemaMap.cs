using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Plaid;

namespace Selah.Domain.Data.SchemaMappings.FinancialAccounts
{
    public class UserInstitutionSchemaMap : EntityMap<UserInstitution>
    {
        public UserInstitutionSchemaMap()
        {
            Map(u => u.Id)
              .ToColumn("id");

            Map(u => u.InstitutionId)
              .ToColumn("institution_id");

            Map(u => u.InstitutionName)
              .ToColumn("institution_name");

            Map(u => u.UserId)
              .ToColumn("user_id");

            Map(u => u.EncryptedAccessToken)
              .ToColumn("encrypted_access_token");
        }
    }
}