//using System.Collections.Generic;
//using TaskManagementSystem.Domain.Entities;

//namespace TaskManagementSystem.Domain.Interfaces
//{
//    public interface ITaskService
//    {
//        Task GetTaskById(int taskId);
//        List<Task> GetTasksByUser(int userId);
//        List<Task> GetOverdueTasksForUser(int userId);
//        int CreateTask(Task task);
//        bool UpdateTask(Task task);
//        bool UpdateTaskStatus(int taskId, string status, int modifiedByUserId);
//        List<Task> SearchTasks(int? assignedToUserId, string status, int pageNumber, int pageSize, out int totalCount);
//        List<User> GetAllMembers();
//        bool CanUserAccessTask(int userId, int taskId, string userRole);
//    }
//}