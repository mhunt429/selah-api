using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Selah.Domain.Data.Models.Transactions
{
    public class UserTransaction
    {
        public int Id { get; set; }

        [Required] public int UserId { get; set; }

        public int AccountId { get; set; }

        [Required] public decimal TransactionAmount { get; set; }

        [Required] public DateTime TransactionDate { get; set; }

        public string MerchantName { get; set; }

        public string ExternalTransactionId { get; set; }

        public string TransactionName { get; set; }

        public bool Pending { get; set; }

        public string PaymentMethod { get; set; }
    }

    public class LineItem
    {
        public long TransactionCategoryId { get; set; }

        public decimal ItemizedAmount { get; set; }

        public bool DefaultCategory { get; set; }

        public long UserId { get; set; }
    }
}