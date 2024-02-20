using System.Threading.Tasks;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Infrastructure.Repository.Interfaces
{
  public interface IAppUserRepository
  {
    public Task<AppUser> GetUser(long id);
    public Task<AppUser> GetUser(string emailOrUsername);
    public Task<bool> UpdateUser(AppUserUpdate updatedUser, long id);
    public Task<int> CreateUser(AppUserCreate createdUser);
    public Task<bool> DeleteUser(long id);
    public Task<bool> UpdatePassword(long userId, string password);
  }
}