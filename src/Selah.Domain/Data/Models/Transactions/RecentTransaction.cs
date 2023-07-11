using System;

namespace Selah.Domain.Data.Models.Transactions
{
    public class RecentTransaction
    {
        /// <summary>
        /// The Id of the transaction
        /// </summary>
        public String TransactionId { get; set; }

        /// <summary>
        /// The date the transaction occured on or was entered via manual entry
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// The total amount of the transaction
        /// </summary>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// The merchant name or location of the transaction
        /// </summary>
        public string Merchant { get; set; }

        /// <summary>
        /// Full name of the account that the user linked the transaction to
        /// </summary>
        public string AccountName { get; set; }
    }
}