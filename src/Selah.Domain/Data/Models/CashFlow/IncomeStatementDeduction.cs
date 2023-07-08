namespace Selah.Domain.Data.Models.CashFlow
{
    public class IncomeStatementDeduction
    {
        public int Id { get; set; }
        public int StatementId { get; set; }

        public string DeductionName { get; set; }

        public decimal Amount { get; set; }
    }

    public class IncomeStatementDeductionCreate
    {
        public int StatementId { get; set; }

        public string DeductionName { get; set; }

        public decimal Amount { get; set; }
    }
}
