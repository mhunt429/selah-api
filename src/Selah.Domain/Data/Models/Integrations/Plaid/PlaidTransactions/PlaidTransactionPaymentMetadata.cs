using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid.PlaidTransactions
{
  public class PlaidTransactionPaymentMetadata
  {
    [JsonProperty("payment_method")]
    public string PaymentMethod { get; set; }
  }
}