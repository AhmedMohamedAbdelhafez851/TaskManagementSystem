using System.Collections.Generic;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Abstraction
{
    public interface ITaskRepository
    {
        // Basic CRUD
        Task GetById(int taskId);
        int Create(Task task);
        bool Update(Task task);
        bool UpdateStatus(int taskId, string status, int modifiedByUserId);

        // Search
        List<Task> SearchTasks(int? assignedToUserId, string status, int pageNumber, int pageSize, out int totalCount);

        // Statistics - For Admin Dashboard
        int GetTaskCount();
        int GetTaskCountByStatus(string status);
        List<Task> GetRecentTasks(int count);

        // For Member Dashboard
        List<Task> GetTasksByUser(int userId);
        List<Task> GetOverdueTasksForUser(int userId);
        int GetTaskCountByUser(int userId);
        int GetTaskCountByUserAndStatus(int userId, string status);

        // For both
        List<User> GetAllMembers();
        User GetUserById(int userId);
    }
}