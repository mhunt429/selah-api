using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Transactions;

namespace Selah.Domain.Data.SchemaMappings.Transactions
{
    public class UserTransactionSchemaMap : EntityMap<UserTransaction>
    {
        public UserTransactionSchemaMap()
        {
            Map(u => u.Id)
              .ToColumn("id");

            Map(u => u.UserId)
              .ToColumn("user_id");

            Map(u => u.AccountId)
              .ToColumn("account_id");

            Map(u => u.TransactionAmount)
              .ToColumn("transaction_amount");

            Map(u => u.TransactionDate)
              .ToColumn("transaction_date");

            Map(u => u.MerchantName)
              .ToColumn("merchant_name");

            Map(u => u.TransactionName)
              .ToColumn("transaction_name");

            Map(u => u.Pending)
              .ToColumn("pending");

            Map(u => u.PaymentMethod)
              .ToColumn("payment_method");

            Map(u => u.ExternalTransactionId)
              .ToColumn("external_transaction_id");
        }
    }
}