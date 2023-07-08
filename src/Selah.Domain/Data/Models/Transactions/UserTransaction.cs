using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Selah.Domain.Data.Models.Transactions
{
    public class UserTransaction
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public int AccountId { get; set; }

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
        public int UserId { get; set; }

        public int AccountId { get; set; }

        public decimal TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string MerchantName { get; set; }

        public string TransactionName { get; set; }

        public bool Pending { get; set; }

        public string PaymentMethod { get; set; }

        public List<LineItem> LineItems { get; set; } = new List<LineItem>();
    }

    public class LineItem
    {
        public int TransactionCategoryId { get; set; }

        public decimal ItemizedAmount { get; set; }

        public bool DefaultCategory { get; set; }
        
        public int UserId { get; set; }
    }
}


