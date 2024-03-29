﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Selah.Domain.Data.Models.CashFlow
{
    public class IncomeStatement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StatementStartDate { get; set; }
        public DateTime StatementEndDate { get; set; }

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
        public DateTime StatementStartDate { get; set; }
        public DateTime StatementEndDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }

        public IReadOnlyCollection<IncomeStatementDeduction> Deductions { get; set; }

        public decimal TotalPay { get; set; }
    }
}
