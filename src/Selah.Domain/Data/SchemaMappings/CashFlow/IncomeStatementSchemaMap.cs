using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.CashFlow;

namespace Selah.Domain.Data.SchemaMappings.CashFlow
{
    public class IncomeStatementSchemaMap : EntityMap<IncomeStatement>
    {
        public IncomeStatementSchemaMap()
        {
            Map(x => x.Id).ToColumn("id");

            Map(x => x.UserId).ToColumn("user_id");

            Map(x => x.StartDate).ToColumn("start_date");

            Map(x => x.EndDate).ToColumn("end_date");
        }
    }
}
