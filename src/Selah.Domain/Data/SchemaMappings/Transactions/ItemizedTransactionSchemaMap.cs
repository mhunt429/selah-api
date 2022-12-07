using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Transactions.Sql;

namespace Selah.Domain.Data.SchemaMappings.Transactions
{
    public class ItemizedTransactionSchemaMap : EntityMap<ItemizedTransactionSql>
    {
        public ItemizedTransactionSchemaMap()
        {
            Map(u => u.Id)
            .ToColumn("id");

            Map(u => u.TransactionId)
            .ToColumn("transaction_id");

            Map(u => u.TransactionCategoryId)
            .ToColumn("transaction_category_id");

            Map(u => u.ItemizedAmount)
            .ToColumn("itemized_amount");

            Map(u => u.AccountId)
            .ToColumn("account_id");

            Map(u => u.UserId)
            .ToColumn("user_id");

            Map(u => u.TransactionAmount)
            .ToColumn("transaction_amount");

            Map(u => u.TransactionDate)
            .ToColumn("transaction_date");

            Map(u => u.MerchantName)
            .ToColumn("merchant_name");

            Map(u => u.TransactionName)
            .ToColumn("merchant_name");

            Map(u => u.Pending)
            .ToColumn("pending");
        }
    }
}
