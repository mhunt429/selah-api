using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid
{
  public class PlaidTokenExchange
  {
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("secret")]
    public string Secret { get; set; }
    
    [JsonProperty("public_token")]
    public string PublicToken { get; set; }
  }

  public class PlaidTokenExchangeResponse
  {
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("item_id")]
    public string ItemId { get; set; }
    
    [JsonProperty("request_id")]
    public string RequestId { get; set; }
  }

  public class PlaidAccountLinkRequest
  {
    public PlaidTokenExchange TokenLink { get; set; }
    
    public PlaidUserInstitutionCreate User { get; set; }
  }
}