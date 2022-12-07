using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts
{
  public class PlaidAccountBalance
  {
    [JsonProperty("available")]
    public decimal? Available { get; set; }
    
    [JsonProperty("current")]
    public decimal? Current { get; set; }
    
    [JsonProperty("iso_currency_code")]
    public string IsoCurrencyCode { get; set; }
  }
}