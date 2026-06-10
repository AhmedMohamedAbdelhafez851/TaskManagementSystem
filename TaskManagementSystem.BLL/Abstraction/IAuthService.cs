using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.BLL.Abstraction
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
        User GetUserById(int userId);
        bool IsUserInRole(int userId, string role);
    }
}