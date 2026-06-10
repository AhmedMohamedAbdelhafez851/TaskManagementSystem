using System.Collections.Generic;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Abstraction
{
    public interface ITaskRepository
    {
        Task GetById(int taskId);
        List<Task> GetTasksByUser(int userId);
        List<Task> GetOverdueTasksForUser(int userId);
        List<Task> SearchTasks(int? assignedToUserId, string status, int pageNumber, int pageSize, out int totalCount);
        int Create(Task task);
        bool Update(Task task);
        bool UpdateStatus(int taskId, string status, int modifiedByUserId);
        List<User> GetAllMembers();
        User GetUserById(int userId);
    }
}