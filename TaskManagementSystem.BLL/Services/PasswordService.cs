using System;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagementSystem.BLL.Services
{
    public class PasswordService
    {
        public void HashPassword(string password, out string hash, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);
            hash = ComputeHash(password, salt);
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string computedHash = ComputeHash(password, storedSalt);
            return string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }

        private string ComputeHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string combined = password + salt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}