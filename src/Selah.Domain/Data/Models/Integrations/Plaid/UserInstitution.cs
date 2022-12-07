using System;

namespace Selah.Domain.Data.Models.Plaid
{
  public record UserInstitution
  {
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }

    public string EncryptedAccessToken { get; set; }
    
    public string InstitutionId { get; set; }
    
    public string InstitutionName { get; set; }
    
    public DateTime ImportedTimestamp { get; set; }
  }

  public record PlaidUserInstitutionCreate
  {
    public Guid UserId { get; set; }
    
    public string EncryptedAccessToken { get; set; }
    
    public string InstitutionId { get; set; }
    
    public string InstitutionName { get; set; }
  }

  /*
   * Not to be confused with the PlaidUserInstitutionCreate, this maps to a client request where the above class
   * maps to a DTO to insert into the database
   */
  public record PlaidUserInstitutionCreateRequest
  {
    public Guid UserId { get; set; }
    
    public string PublicToken { get; set; }
    
    public string InstitutionId { get; set; }
    
    public string InstitutionName { get; set; }
    
  }
}