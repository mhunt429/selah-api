using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Integrations.TDAmeritrade
{
  public record OAuthTokenRequest
  {
    [JsonProperty("code")]
    public string Code { get; set; }
  }

  public record OAuthTokenResponse
  {
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
   
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
   
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }
    
    [JsonProperty("scope")]
    public string Scope { get; set; }
    
    [JsonProperty("refresh_token_expires_in")]
    public long RefreshTokenExpiresIn { get; set; }
  }
}