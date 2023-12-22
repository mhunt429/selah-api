using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Selah.Domain.Data.Models.Plaid;

public class TokenLinkRequest
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    
    [JsonPropertyName("secret")]
    public string Secret { get; set; }
    
    [JsonPropertyName("client_name")]
    public string ClientName { get; set; }
    
    [JsonPropertyName("country_codes")]
    public List<string> CountryCodes { get; set; }
    
    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("products")] public IReadOnlyList<string> Products { get; set; }
        = new List<string> { "auth", "balance", "transactions" };
    
    [JsonPropertyName("user")]
    public TokenLinkUser User { get; set; }
}

public class TokenLinkUser
{
    [JsonPropertyName("client_user_id")]
    public string ClientUserId { get; set; }
}

public class TokenLinkResponse
{
    [JsonPropertyName("expiration")]
    public string Expiration { get; set; }
    
    [JsonPropertyName("link_token")]
    public string LinkToken { get; set; }
    
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; }
}
