using System;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Transactions;

public class TransactionCreate
{
    public string UserId { get; set; }

    public string AccountId { get; set; }

    public decimal TransactionAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string MerchantName { get; set; }

    public string TransactionName { get; set; }

    public bool Pending { get; set; }

    public string PaymentMethod { get; set; }

    public string RecurringTransactionId { get; set; }
    public List<LineItem> LineItems { get; set; } = new List<LineItem>();
}


public class TransactionCreateSql
{
    public long UserId { get; set; }

    public long AccountId { get; set; }

    public decimal TransactionAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string MerchantName { get; set; }

    public string TransactionName { get; set; }
    public bool Pending { get; set; }

    public string PaymentMethod { get; set; }
    
   public long RecurringTransactionId { get; set; }
}