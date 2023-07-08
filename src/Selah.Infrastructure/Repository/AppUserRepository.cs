using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly IBaseRepository _baseRepository;

        public AppUserRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<IEnumerable<AppUser>> GetUsers(int limit, int offset)
        {
            var sql = @"SELECT id, email, user_name, password, 
       first_name, last_name, date_created FROM app_user ORDER BY last_name, first_name, date_created";

            return await _baseRepository.GetAllAsync<AppUser>(sql, null);
        }

        public async Task<AppUser> GetUser(Guid id)
        {
            var sql = @"SELECT id, email, user_name, password, 
                        first_name, last_name, date_created
                        FROM app_user WHERE id = @id";

            return await _baseRepository.GetFirstOrDefaultAsync<AppUser>(sql, new { id });
        }

        // Handles initial authentication request
        public async Task<AppUser> GetUser(string emailOrUsername)
        {
            var sql = @"SELECT id, email, user_name, password, 
                     first_name, last_name, date_created FROM 
                     app_user WHERE (email = @email OR user_name = @user_name)";

            return await _baseRepository.GetFirstOrDefaultAsync<AppUser>(sql, new { email = emailOrUsername, user_name = emailOrUsername });
        }

        public async Task<int> CreateUser(AppUserCreate createdUser)
        {
            var sql = @"INSERT INTO app_user(email, 
                     user_name, 
                     password, 
                     first_name, 
                     last_name, 
                     date_created)
                     values(
                              @email,
                              @user_name,
                              @password, 
                              @first_name, 
                              @last_name, 
                              @date_created
                            )returning(id)";
            var parameters = new
            {
                email = createdUser.Email,
                user_name = createdUser.UserName,
                password = createdUser.Password,
                first_name = createdUser.FirstName,
                last_name = createdUser.LastName,
                date_created = DateTime.UtcNow
            };

            return await _baseRepository.AddAsync<int>(sql, parameters);
        }

        public async Task UpdateUser(AppUserUpdate updatedUser, Guid id)
        {
            var sql = @"UPDATE app_user 
                    SET email = @email,
                    username = @user_name,
                    first_name = @first_name,
                    last_name = @last_name
                    WHERE id = @id";

            var parameters = new
            {
                updatedUser.Email,
                updatedUser.FirstName,
                updatedUser.LastName,
                id
            };

            await _baseRepository.UpdateAsync(sql, parameters);
        }

        public async Task DeleteUser(Guid id)
        {
            var sql = "DELETE FROM app_user WHERE id = @id";
            await _baseRepository.DeleteAsync(sql, new { id });
        }

        public async Task UpdatePassword(int userId, string password)
        {
            var sql = @"UPDATE app_user 
                    SET password = @password
                    WHERE id = @id";

            var parameters = new
            {
                id = userId,
                password
            };

            await _baseRepository.UpdateAsync(sql, parameters);
        }
    }
}