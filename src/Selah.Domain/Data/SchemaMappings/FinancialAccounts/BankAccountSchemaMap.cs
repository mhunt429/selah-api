using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Banking;

namespace Selah.Domain.Data.SchemaMappings.FinancialAccounts
{
    public class BankAccountSchemaMap : EntityMap<BankAccountSql>
    {
        public BankAccountSchemaMap()
        {
            Map(b => b.Id)
              .ToColumn("id");

            Map(b => b.AccountMask)
              .ToColumn("account_mask");

            Map(b => b.AccountName)
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