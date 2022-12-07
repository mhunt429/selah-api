using System;
using System.ComponentModel.DataAnnotations;

namespace Selah.Domain.Data.Models.Transactions
{
  public class UserTransaction
  {
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public Guid AccountId { get; set; }
    
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
}