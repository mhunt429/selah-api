using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid.PlaidTransactions
{
  public class PlaidTransactionRequestOptions
  {
    [JsonProperty("count")]
    public int Count { get; set; }
  }
}