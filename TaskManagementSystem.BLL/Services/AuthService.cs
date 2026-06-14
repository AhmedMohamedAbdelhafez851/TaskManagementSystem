using System;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.BLL.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public AuthService()
        {
            _userRepository = new UserRepository();
            _passwordService = new PasswordService();
        }

        public AuthService(UserRepository userRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                var user = _userRepository.GetByUsername(username);

                if (user == null || !user.IsActive)
                    return null;

                bool isPasswordValid = _passwordService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordValid)
                    return null;

                _userRepository.UpdateLastLoginDate(user.UserId);

                // Remove sensitive data
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