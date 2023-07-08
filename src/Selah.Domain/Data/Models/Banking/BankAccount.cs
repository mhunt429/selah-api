using System;

namespace Selah.Domain.Data.Models.Banking
{
  public record BankAccount
  {
    public int Id { get; set; }
    
    public string ExternalAccountId { get; set; }
    
    public string Mask { get; set; }
    
    public string Name { get; set; }
    
    public decimal? AvailableBalance { get; set; }
    
    public decimal? CurrentBalance { get; set; }
    
    public int UserId { get; set; }
    
    public string Subtype { get; set; }
    
    public int InstitutionId { get; set; }// Foreign reference to a user institution primary key
  }
}