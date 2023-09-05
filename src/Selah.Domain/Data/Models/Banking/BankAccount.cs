namespace Selah.Domain.Data.Models.Banking
{
  /// <summary>
  /// Canonical mappings to the database schema
  /// </summary>
  public record BankAccountSql
  {
    public int Id { get; set; }

    public string AccountMask { get; set; }
    
    public string Name { get; set; }
    
    public decimal? AvailableBalance { get; set; }
    
    public decimal? CurrentBalance { get; set; }
    
    public int UserId { get; set; }
    
    public string Subtype { get; set; }
    
    public int InstitutionId { get; set; }// Foreign reference to a user institution primary key
  }
  
  public record BankAccount
  {
    public string Id { get; set; }

    public string AccountMask { get; set; }
    
    public string Name { get; set; }
    
    public decimal? AvailableBalance { get; set; }
    
    public decimal? CurrentBalance { get; set; }
    
    public string UserId { get; set; }
    
    public string Subtype { get; set; }
    
    public string InstitutionId { get; set; }// Foreign reference to a user institution primary key
  }
  
  //Use case is when a user creates a transaction, we can update the balance directly through this
  public class BalanceUpdateSql
  {
    public int AccountId { get; set; }
    
    public decimal AvailableBalance { get; set; }
    
    public decimal CurrentBalance { get; set; }
  }
  
  //Allows a user to update the balances without sending over the entire payload 
  //We can update this when the user creates a transaction say for a credit card or does an ACH draft
  public class BalanceUpdate
  {
    public string AccountId { get; set; }
    
    public decimal AvailableBalance { get; set; }
    
    public decimal CurrentBalance { get; set; }
  }
}