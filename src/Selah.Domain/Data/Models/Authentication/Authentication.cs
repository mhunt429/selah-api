using System;
using System.Text.Json.Serialization;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Domain.Data.Models.Authentication
{
  public record AuthenticationRequest
  {
    [JsonPropertyName("emailOrUsername")]
    public string EmailOrUsername { get; set; }
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
  }

  public record AuthenticationResponse
  {
    [JsonPropertyName("user")]
    public UserViewModel User { get; set; }
    
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expiration_ts")]
    public DateTime ExpirationTs { get; set; }
  }

  public record JwtResponse
  {
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expiration_ts")]
    public DateTime ExpirationTs { get; set; }
  }
}