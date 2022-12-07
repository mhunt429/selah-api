using System;

namespace Selah.Domain.Data.Models.Transactions
{
    public class TransactionLineItemCreate
    {
        public Guid TransactionId { get; set; }

        //On Plaid imports, we create a null line item because we have no way to auto-categorize transaction at the moment
        public Guid? TransactionCategoryId { get; set; }
        public decimal ItemizedAmount { get; set; }

        //If the transaction was imported from Plaid
        public string ExternalTransactionId { get; set; }
    }
}
