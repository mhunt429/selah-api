using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Services
{
    public class UserService : IUserService
    {
        //TODO add logging and data validation
        private readonly IAppUserRepository _userRepository;

        public UserService(IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<AppUser>> GetUsers(int limit, int offset)
        {
            return await _userRepository.GetUsers(limit, offset);
        }

        public async Task<AppUser> GetUser(Guid id)
        {
            return await _userRepository.GetUser(id);
        }

      

        public async Task<AppUser> CreateUser(AppUserCreate user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var userId = await _userRepository.CreateUser(user);

            return new AppUser
            {
                Id = userId,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateCreated = DateTime.UtcNow,
            };
        }

        public async Task<AppUser> UpdateUser(AppUserUpdate updatedUser, Guid id)
        {
            var currentUser = await _userRepository.GetUser(id);
            if (currentUser == null)
            {
                return null;
            }

            try
            {
                await _userRepository.UpdateUser(updatedUser, id);
                return new AppUser
                {
                    Id = currentUser.Id,
                    FirstName = updatedUser.FirstName,
                    LastName = updatedUser.LastName,
                    DateCreated = currentUser.DateCreated
                };
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task DeleteUser(Guid id)
        {
            await _userRepository.DeleteUser(id);
        }

        /// <summary>
        /// TODO add a TFA check if the user does not supply the current password when updating a password
        /// There will eventually be a forgot password flow that uses a 1 time token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordUpdate"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePassword(Guid userId, PasswordUpdate passwordUpdate)
        {
            if (passwordUpdate.Password != passwordUpdate.PasswordConfirmation)
            {
                return false;
            }
            var passwordUpdateWithSalt = passwordUpdate with { Password = BCrypt.Net.BCrypt.HashPassword(passwordUpdate.Password) };
            await _userRepository.UpdatePassword(userId, passwordUpdateWithSalt.Password);
            return true;
        }
    }
}