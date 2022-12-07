namespace Selah.Domain.Data.Models.ApplicationUser
{
  public record PasswordUpdate
  {
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
  }
}