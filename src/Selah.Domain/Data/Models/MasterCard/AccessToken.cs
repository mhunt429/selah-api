
using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.MasterCard;

public class AccessToken
{
    [JsonProperty("token")]
    public string Token { get; set; }
}