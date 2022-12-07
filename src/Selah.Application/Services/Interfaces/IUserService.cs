using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Application.Services.Interfaces
{
  public interface IUserService
  {
    public Task<IEnumerable<AppUser>> GetUsers(int limit, int offset);
    public Task<AppUser> GetUser(Guid id);
    public Task<AppUser> CreateUser(AppUserCreate user);
    public Task<AppUser> UpdateUser(AppUserUpdate user, Guid id);
    public Task DeleteUser(Guid id);
    public Task<bool> UpdatePassword(Guid userId, PasswordUpdate passwordUpdate);
  }
}