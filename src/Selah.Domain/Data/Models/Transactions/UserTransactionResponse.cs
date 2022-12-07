using System;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Transactions
{
  public class UserTransactionResponse
  {
    public string TransactionDate { get; set; }
    public long Records { get; set; }
    public List<UserTransactionVM> Transactions { get; set; }
    
    public decimal TotalTransactionAmount { get; set; }
  }
}