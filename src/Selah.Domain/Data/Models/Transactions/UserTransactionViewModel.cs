using System;

namespace Selah.Domain.Data.Models.Transactions
{
  public class UserTransactionQueryResult
  {
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string MerchantName { get; set; }
    public string ExternalTransactionId { get; set; }
    public decimal TransactionAmount { get; set; }
    public int Records { get; set; }
    public string TransactionName { get; set; }
  }

  public class UserTransactionVM
  {
    public string Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string MerchantName { get; set; }
    public string ExternalTransactionId { get; set; }
    public decimal TransactionAmount { get; set; }
    
    public string TransactionName { get; set; }
  }
}