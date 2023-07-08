using System;

namespace Selah.Domain.Data.Models.Transactions
{
    public class TransactionLineItemCreate
    {
        public int TransactionId { get; set; }

        public int? TransactionCategoryId { get; set; }

        public decimal ItemizedAmount { get; set; }

        public bool DefaultCategory { get; set; }
    }
}
