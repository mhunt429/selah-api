using System;
using System.Collections.Generic;
using System.Linq;

namespace Selah.Domain.Data.Models.CashFlow
{
    public class IncomeStatement
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IReadOnlyCollection<IncomeStatementDeduction> Deductions { get; set; }

        public decimal TotalPay { get; set; }

        public decimal TakeHomePay
        {
            get
            {
                return TotalPay - Deductions.Sum(x => x.Amount);
            }
        }
    }

    public class IncomeStatementCreate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }

        public IReadOnlyCollection<IncomeStatementDeduction> Deductions { get; set; }

        public decimal TotalPay { get; set; }
    }
}
