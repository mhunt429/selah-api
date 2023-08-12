using System;

namespace Selah.Domain.Data.Models.Transactions.Sql
{
    public class ItemizedTransactionSql
    {
        //Transaction Line Item
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int TransactionCategoryId { get; set; }
        public decimal ItemizedAmount { get; set; }

        //Transaction Fields

        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MerchantName { get; set; }
        public string TransactionName { get; set; }
        public bool Pending { get; set; }
    }
}
