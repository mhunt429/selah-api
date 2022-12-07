namespace Selah.Domain.Data.Models.ApplicationUser
{
  public record AppUserUpdate
  {
    public string Email { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}