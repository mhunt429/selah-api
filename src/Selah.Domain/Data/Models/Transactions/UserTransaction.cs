using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Selah.Domain.Data.Models.Transactions
{
    public class UserTransaction
    {
        public long Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public Guid AccountId { get; set; }

        [Required]
        public decimal TransactionAmount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public string MerchantName { get; set; }

        public string ExternalTransactionId { get; set; }

        public string TransactionName { get; set; }

        public bool Pending { get; set; }

        public string PaymentMethod { get; set; }
    }
    public class TransactionCreate
    {
        public Guid UserId { get; set; }

        public long AccountId { get; set; }

        public decimal TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string MerchantName { get; set; }

        public string TransactionName { get; set; }

        public bool Pending { get; set; }

        public string PaymentMethod { get; set; }

        public IReadOnlyCollection<LineItem> LineItems { get; set; }
    }

    public class LineItem
    {
        public long TransactionCategoryId { get; set; }

        public decimal ItemizedAmount { get; set; }

        public bool DefaultCategory { get; set; }
    }
}


