using System.Collections.Generic;
using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid
{
  public class PlaidTokenLinkRequest
  {
    [JsonProperty("client_id")]
    public string  ClientId { get; set; }
    
    [JsonProperty("secret")]
    public string  Secret { get; set; }
    
    [JsonProperty("client_name")]
    public string  ClientName { get; set; }
    
    [JsonProperty("country_codes")]
    public List<string> CountryCodes { get; set; }
    
    [JsonProperty("language")]
    public string Language { get; set; }
    
    [JsonProperty("user")]
    public PlaidTokenLinkUser User { get; set; }
    
    [JsonProperty("products")]
    public List<string> Products { get; set; }
  }

  public class PlaidTokenLinkUser
  {
    [JsonProperty("client_user_id")]
    public string ClientUserId { get; set; }
  }

  public class PlaidTokenLinkResponse
  {
    [JsonProperty("expiration")]
    public string Expiration { get; set; }
    
    [JsonProperty("link_token")]
    public string LinkToken { get; set; }
    
    [JsonProperty("request_id")]
    public string RequestId { get; set; }
  }
}