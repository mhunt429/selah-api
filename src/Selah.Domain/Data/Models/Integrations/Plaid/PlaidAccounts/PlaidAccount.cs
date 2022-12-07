using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts
{
  public class PlaidAccount
  {
    [JsonProperty("account_id")]
    public string AccountId { get; set; }
    
    [JsonProperty("balances")]
    public PlaidAccountBalance Balances { get; set; }
    
    [JsonProperty("mask")]
    public string Mask { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("official_name")]
    public string OfficialName { get; set; }
    
    [JsonProperty("subtype")]
    public string Subtype { get; set; }
  }
}