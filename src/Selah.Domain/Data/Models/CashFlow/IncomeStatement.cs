using System;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.CashFlow
{
    public class IncomeStatement
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public Guid UserId { get; set; }

        public IReadOnlyCollection<IncomeStatementDeduction> Deductions { get; set; }

        public decimal TakeHomePay { get; set; }
    }
}
