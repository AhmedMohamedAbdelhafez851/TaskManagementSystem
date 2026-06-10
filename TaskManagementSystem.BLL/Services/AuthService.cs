using System;
using TaskManagementSystem.BLL.Abstraction;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public AuthService()
        {
            _userRepository = new UserRepository();
            _passwordService = new PasswordService();
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                var user = _userRepository.GetByUsername(username);

                if (user == null)
                    return null;

                if (!user.IsActive)
                    return null;

                bool isPasswordValid = _passwordService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordValid)
                    return null;

                _userRepository.UpdateLastLoginDate(user.UserId);

                // Remove sensitive data before returning
                user.PasswordHash = null;
                user.PasswordSalt = null;

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Authentication failed for user {username}: {ex.Message}", ex);
            }
        }

        public User GetUserById(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                user.PasswordHash = null;
                user.PasswordSalt = null;
            }
            return user;
        }

        public bool IsUserInRole(int userId, string role)
        {
            var user = _userRepository.GetById(userId);
            return user != null && user.Role == role;
        }
    }
}