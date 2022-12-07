using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Transactions.Sql;

namespace Selah.Domain.Data.SchemaMappings.Transactions
{
    public class RecentTransactionSchemaMap : EntityMap<RecentTransactionSql>
    {
        public RecentTransactionSchemaMap()
        {
            Map(u => u.TransactionId).ToColumn("transaction_id");
            Map(u => u.TransactionDate).ToColumn("transaction_date");
            Map(u => u.Merchant).ToColumn("merchant_name");
            Map(u => u.TransactionAmount).ToColumn("transaction_amount");
            Map(u => u.AccountName).ToColumn("account_name");
        }
    }
}
