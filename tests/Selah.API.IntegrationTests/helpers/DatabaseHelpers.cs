using Microsoft.Extensions.Logging;
using MySqlConnector;
using NSubstitute;
using Respawn;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure;
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
            int userId = await userRepo.CreateUser(user);
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

        public static async Task DeleteTestUsers(BaseRepository repo, int id)
        {
            string sql = $"DELETE FROM app_user WHERE id ='{id}'";
            await repo.DeleteAsync(sql, null);
        }

        //Used for specific cleanup tasks;
        public static async Task RunSingleDelete(BaseRepository repo, string sql, object parameters)
        {
            await repo.DeleteAsync(sql, parameters);
        }

        //Sets up all the connection string and dependency injection for testing the database layer
        public static BaseRepository CreateBaseRepository()
        {
            ILogger<BaseRepository> loggerMock = Substitute.For<ILogger<BaseRepository>>();

            return new BaseRepository(new MySqlConnectionFactory(BuildTestConnectionString()), loggerMock);
        }

        public static async Task ResetDb()
        {
            using (var conn = new MySqlConnection(BuildTestConnectionString()))
            {
                await conn.OpenAsync();
                var respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
                {
                    SchemasToInclude = new[]
                    {
                        "selah_db"
                    },
                    DbAdapter = DbAdapter.MySql
                });
                await respawner.ResetAsync(conn);
            }
        }

        private static string BuildTestConnectionString()
        {
            //DB_CONNECTION_STRING is set as an environment variable in the GitHub actions pipelines.
            //The value below is whatever port you are using for the local postgres image
            var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                dbConnectionString = "User ID=mysqluser;Password=mysqlpassword;Host=localhost;Database=selah_db";
            }

            return dbConnectionString;
        }
    }
}