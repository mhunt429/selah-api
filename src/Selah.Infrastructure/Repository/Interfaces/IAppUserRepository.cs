using System.Threading.Tasks;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Infrastructure.Repository.Interfaces
{
  public interface IAppUserRepository
  {
    public Task<AppUser> GetUser(int id);
    public Task<AppUser> GetUser(string emailOrUsername);
    public Task<bool> UpdateUser(AppUserUpdate updatedUser, int id);
    public Task<int> CreateUser(AppUserCreate createdUser);
    public Task<bool> DeleteUser(int id);
    public Task<bool> UpdatePassword(int userId, string password);
  }
}