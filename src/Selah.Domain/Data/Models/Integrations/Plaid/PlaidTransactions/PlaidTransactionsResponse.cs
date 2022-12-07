using System.Collections.Generic;
using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid.PlaidTransactions
{
  public class PlaidTransactionsResponse
  {
    [JsonProperty("transactions")]
    public IEnumerable<PlaidTransaction> Transactions { get; set; }
    
    [JsonProperty("total_transactions")]
    public int TotalTransactions { get; set; }
  }
}