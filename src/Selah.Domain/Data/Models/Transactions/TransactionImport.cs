using System;

namespace Selah.Domain.Data.Models.Transactions;

public class TransactionImport
{
    public string UserId { get; set; }
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
    
}