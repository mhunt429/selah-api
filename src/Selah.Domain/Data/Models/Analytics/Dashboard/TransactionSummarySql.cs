using System;

namespace Selah.Domain.Data.Models.Analytics.Dashboard
{
    public class TransactionSummarySql
    {
        public DateTime TransactionDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int Count { get; set; }
    }
}
