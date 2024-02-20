namespace Selah.Domain.Data.Models.Banking;

public class BankAccountCreate
{
    public string AccountMask { get; set; }
    
    public string Name { get; set; }
    
    public decimal? AvailableBalance { get; set; }
    
    public decimal? CurrentBalance { get; set; }
    
    public long UserId { get; set; }
    
    public string Subtype { get; set; }
    
    public long InstitutionId { get; set; }
}

public class BankAccountUpdate
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



