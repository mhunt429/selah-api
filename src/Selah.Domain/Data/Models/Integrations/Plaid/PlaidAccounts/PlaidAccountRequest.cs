using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts
{
  public class PlaidAccountRequest
  {
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("secret")]
    public string Secret { get; set; }
    
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
  }
}