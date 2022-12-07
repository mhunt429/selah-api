using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Banking;

namespace Selah.Domain.Data.SchemaMappings.FinancialAccounts
{
    public class BankAccountSchemaMap : EntityMap<BankAccount>
    {
        public BankAccountSchemaMap()
        {
            Map(b => b.Id)
              .ToColumn("id");

            Map(b => b.ExternalAccountId)
              .ToColumn("external_account_id");

            Map(b => b.Mask)
              .ToColumn("account_mask");

            Map(b => b.Name)
              .ToColumn("account_name");

            Map(b => b.AvailableBalance)
              .ToColumn("available_balance");

            Map(b => b.CurrentBalance)
              .ToColumn("current_balance");

            Map(b => b.UserId)
              .ToColumn("user_id");

            Map(b => b.Subtype)
              .ToColumn("subtype");

            Map(b => b.InstitutionId)
              .ToColumn("institution_id");
        }
    }
}