using System;

namespace Selah.Domain.Data.Models.ApplicationUser
{
  public record AppUserCreate
  {
    public string Email { get; set; }
    
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
  }
}