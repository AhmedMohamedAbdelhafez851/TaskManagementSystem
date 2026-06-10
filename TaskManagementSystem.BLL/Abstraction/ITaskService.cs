using System.Collections.Generic;
using TaskManagementSystem.BLL.DTOs;

namespace TaskManagementSystem.BLL.Abstraction
{
    public interface ITaskService
    {
        TaskDto GetTaskById(int taskId);
        List<TaskDto> GetTasksByUser(int userId);
        List<TaskDto> GetOverdueTasksForUser(int userId);
        int CreateTask(CreateTaskDto taskDto, int createdByUserId, string uploadFolderPath);
        bool UpdateTask(UpdateTaskDto taskDto, int modifiedByUserId, string uploadFolderPath);
        bool UpdateTaskStatus(int taskId, string status, int modifiedByUserId);
        SearchResultDto SearchTasks(SearchCriteriaDto criteria);
        List<UserDto> GetAllMembers();
        bool CanUserAccessTask(int userId, int taskId, string userRole);
        UserDto GetUserById(int userId);
    }
}