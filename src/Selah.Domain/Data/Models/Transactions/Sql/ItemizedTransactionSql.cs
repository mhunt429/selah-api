using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selah.Domain.Data.Models.Transactions.Sql
{
    public class ItemizedTransactionSql
    {
        //Transaction Line Item
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid TransactionCategoryId { get; set; }
        public decimal ItemizedAmount { get; set; }

        //Transaction Fields

        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MerchantName { get; set; }
        public string TransactionName { get; set; }
        public bool Pending { get; set; }
    }
}
