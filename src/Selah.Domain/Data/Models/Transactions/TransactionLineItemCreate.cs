namespace Selah.Domain.Data.Models.Transactions
{
    public class TransactionLineItemCreate
    {
        public long TransactionId { get; set; }

        public long? TransactionCategoryId { get; set; }

        public decimal ItemizedAmount { get; set; }

        public bool DefaultCategory { get; set; }
    }
}
