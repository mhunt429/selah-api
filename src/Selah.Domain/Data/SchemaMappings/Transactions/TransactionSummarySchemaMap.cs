using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Analytics.Dashboard;

namespace Selah.Domain.Data.SchemaMappings.Transactions
{
    public class TransactionSummarySchemaMap : EntityMap<TransactionSummarySql>
    {
        public TransactionSummarySchemaMap()
        {
            Map(u => u.TransactionDate)
            .ToColumn("transaction_date");

            Map(u => u.TotalAmount)
            .ToColumn("total_amount");

            Map(u => u.Count)
            .ToColumn("count");
        }
    }
}
