using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Application.Services.Interfaces
{
  public interface IUserService
  {
    public Task<UserViewModel> GetUser(Guid id);
    public Task<UserViewModel> CreateUser(AppUserCreate user);
    public Task<UserViewModel> UpdateUser(AppUserUpdate user, Guid id);
    public Task DeleteUser(Guid id);
    public Task<bool> UpdatePassword(Guid userId, PasswordUpdate passwordUpdate);
  }
}