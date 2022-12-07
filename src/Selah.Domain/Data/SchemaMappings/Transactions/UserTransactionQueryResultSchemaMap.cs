using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Transactions;

namespace Selah.Domain.Data.SchemaMappings.Transactions
{
    public class UserTransactionQueryResultSchemaMap : EntityMap<UserTransactionQueryResult>
    {
        public UserTransactionQueryResultSchemaMap()
        {
            Map(t => t.Id)
              .ToColumn("id");

            Map(t => t.Records)
              .ToColumn("records");

            Map(t => t.MerchantName)
              .ToColumn("merchant_name");

            Map(t => t.TransactionAmount)
              .ToColumn("transaction_amount");

            Map(t => t.TransactionDate)
              .ToColumn("transaction_date");

            Map(t => t.TransactionName)
              .ToColumn("transaction_name");

            Map(t => t.ExternalTransactionId)
              .ToColumn("external_transaction_id");
        }
    }
}