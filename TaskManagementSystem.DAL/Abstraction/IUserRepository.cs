using System.Collections.Generic;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Abstraction
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets user by username (without password verification)
        /// Password verification should happen in BLL
        /// </summary>
        User GetByUsername(string username);
        User GetById(int userId);
        List<User> GetAllMembers();
        void UpdateLastLoginDate(int userId);
    }
}