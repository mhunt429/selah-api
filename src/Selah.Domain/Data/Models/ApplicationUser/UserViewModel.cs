using System;

namespace Selah.Domain.Data.Models.ApplicationUser
{
  public record UserViewModel
  {
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateCreated { get; set; }
  }
}