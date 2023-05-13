namespace Selah.Domain.Data.Models.CashFlow
{
    public class IncomeStatementDeduction
    {
        public long Id { get; set; }
        public long IncomeStatementId { get; set; }

        public string DeductionName { get; set; }

        public decimal Amount { get; set; }
    }

    public class IncomeStatementDeductionCreate
    {
        public long IncomeStatementId { get; set; }

        public string DeductionName { get; set; }

        public decimal Amount { get; set; }
    }
}
