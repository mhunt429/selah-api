using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.CashFlow;

namespace Selah.Domain.Data.SchemaMappings.CashFlow
{
    public class IncomeStatementDeductionSchemaMap : EntityMap<IncomeStatementDeduction>
    {
        public IncomeStatementDeductionSchemaMap()
        {
            Map(x => x.Id).ToColumn("id");

            Map(x => x.StatementId).ToColumn("statement_id");

            Map(x => x.DeductionName).ToColumn("deduction_name");

            Map(x => x.Amount).ToColumn("amount");
        }
    }
}
