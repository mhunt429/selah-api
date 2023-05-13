using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository;
namespace Selah.API.IntegrationTests.helpers
{
    public static class DatabaseHelpers
    {
        public static async Task<Guid> CreateUser(AppUserRepository userRepo)
        {
            var user = new AppUserCreate
            {
                Email = "test@selah.com",
                FirstName = "Test",
                LastName = "User",
                UserName = "test_123",
            };
            //The "minimal foreign key constraint is a valid user"
            user.Password = BCrypt.Net.BCrypt.HashPassword("password");
            Guid userId = await userRepo.CreateUser(user);
            return userId;
        }

        public static async Task DeleteUser(BaseRepository repo, Guid userId)
        {
            string sql = "DELETE FROM app_user WHERE id = @id";
            await repo.DeleteAsync(sql, new { id = userId });
        }

        //Used for specific cleanup tasks;
        public static async Task RunSingleDelete(BaseRepository repo, string sql, object parameters)
        {
            await repo.DeleteAsync(sql, parameters);
        }
    }
}
