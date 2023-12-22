using System.Text.Json.Serialization;

namespace Selah.Domain.Data.Models.Plaid;

public class TokenExchange
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    
    [JsonPropertyName("secret")]
    public string Secret { get; set; }
    
    [JsonPropertyName("public_token")]
    public string PublicToken { get; set; }
}