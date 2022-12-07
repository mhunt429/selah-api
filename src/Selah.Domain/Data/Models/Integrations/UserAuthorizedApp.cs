

using System;

namespace Selah.Domain.Data.Models.Integrations
{
  public record UserAuthorizedApp
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public string Scope { get; set; }
    public string EncryptedAccessToken { get; set; }
    public string EncryptedRefreshToken { get; set; }
    public DateTime AccessTokenExpirationTs { get; set; }
    public DateTime RefreshTokenExpirationTs { get; set; }
    public DateTime CreatedTs { get; set; }
    public DateTime UpdatedTs { get; set; }
  }

  //Only exposed type on an API request
  public record UserAuthorizedAppViewModel
  {
    public string Scope { get; set; }
    public Guid ApplicationId { get; set; }
    public DateTime CreatedTs { get; set; }
    public DateTime UpdatedTs { get; set; }
  }
  
  public record UserAuthorizedAppCreate
  {
    public Guid UserId { get; set; }
    public string Scope { get; set; }
    public string EncryptedAccessToken { get; set; }
    public string EncryptedRefreshToken { get; set; }
    public DateTime AccessTokenExpirationTs { get; set; }
    public DateTime RefreshTokenExpirationTs { get; set; }
    public DateTime CreatedTs { get; set; }
    public DateTime UpdatedTs { get; set; }
    public DateTime EpochToDate(long millis){
      return DateTime.UtcNow.AddSeconds(millis);
    }
  }
}