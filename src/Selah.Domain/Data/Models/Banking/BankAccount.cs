using System;

namespace Selah.Domain.Data.Models.Banking
{
  public record BankAccount
  {
    public Guid Id { get; set; }
    
    public string ExternalAccountId { get; set; }
    
    public string Mask { get; set; }
    
    public string Name { get; set; }
    
    public decimal? AvailableBalance { get; set; }
    
    public decimal? CurrentBalance { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Subtype { get; set; }
    
    public Guid InstitutionId { get; set; }// Foreign reference to a user institution primary key
  }
}