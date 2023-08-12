using System;

namespace Selah.Domain.Data.Models.Transactions.Commands
{
    public class TransactionCreateResponse
    {
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal TranscationAmount { get; set; }

        public int LineItems { get; set; }
    }
}
