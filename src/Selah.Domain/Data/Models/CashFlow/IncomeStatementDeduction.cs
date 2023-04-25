using System;

namespace Selah.Domain.Data.Models.CashFlow
{
    public class IncomeStatementDeduction
    {
        public long IncomeStatementId { get; set; }
        
        public string Category { get; set; }

        public decimal Amount { get; set; }
    }
}
