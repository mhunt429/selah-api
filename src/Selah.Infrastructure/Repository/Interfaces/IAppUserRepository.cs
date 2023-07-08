using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Infrastructure.Repository.Interfaces
{
  public interface IAppUserRepository
  {
    public Task<IEnumerable<AppUser>> GetUsers(int limit, int offset);
    public Task<AppUser> GetUser(Guid id);
    public Task<AppUser> GetUser(string emailOrUsername);
    public Task UpdateUser(AppUserUpdate updatedUser, Guid id);
    public Task<int> CreateUser(AppUserCreate createdUser);
    public Task DeleteUser(Guid id);
    public Task UpdatePassword(int userId, string password);
  }
}