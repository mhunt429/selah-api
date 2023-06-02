using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository;
namespace Selah.API.IntegrationTests.helpers
{
    public static class DatabaseHelpers
    {
        public static async Task<AppUser> CreateUser(AppUserRepository userRepo)
        {
            var user = new AppUserCreate
            {
                Email = $"{Guid.NewGuid()}@selah.com",
                FirstName = "Test",
                LastName = "User",
                UserName = $"{Guid.NewGuid()}",
                Password = "password"
            };
            //The "minimal foreign key constraint is a valid user"
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            Guid userId = await userRepo.CreateUser(user);
            return new AppUser
            {
                Id = userId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Password = "password"
            };
        }

        public static async Task DeleteTestUsers(BaseRepository repo)
        {
            string sql = "DELETE FROM app_user WHERE email LIKE '%selah.com%'";
            await repo.DeleteAsync(sql, null);
        }

        //Used for specific cleanup tasks;
        public static async Task RunSingleDelete(BaseRepository repo, string sql, object parameters)
        {
            await repo.DeleteAsync(sql, parameters);
        }
    }
}
