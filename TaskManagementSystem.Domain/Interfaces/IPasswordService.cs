namespace TaskManagementSystem.Domain.Interfaces
{
    public interface IPasswordService
    {
        void HashPassword(string password, out string hash, out string salt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
    }
}