namespace Selah.Domain.Data.Models.MasterCard;

public class CustomerCreate
{
    public string Username { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string ApplicationId { get; set; }
    
    public string Phone { get; set; }
    
    public string Email { get; set; }
}