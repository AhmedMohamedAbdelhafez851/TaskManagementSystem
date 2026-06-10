using System;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagementSystem.BLL.Services
{
    public class PasswordService
    {
        // Hash a password with a generated salt
        public void HashPassword(string password, out string hash, out string salt)
        {
            // Generate a random salt (16 bytes)
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Compute hash
            hash = ComputeHash(password, salt);
        }

        // Verify a password against stored hash and salt
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string computedHash = ComputeHash(password, storedSalt);
            return computedHash == storedHash;
        }

        // Compute SHA256 hash of password + salt
        private string ComputeHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string combined = password + salt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}