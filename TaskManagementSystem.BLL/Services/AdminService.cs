using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.BLL.Abstraction;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Constants;

namespace TaskManagementSystem.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;

        public AdminService()
        {
            _taskRepository = new TaskRepository();
            _userRepository = new UserRepository();
        }

        public DashboardStatisticsDto GetDashboardStatistics()
        {
            try
            {
                return new DashboardStatisticsDto
                {
                    TotalTasks = _taskRepository.GetTaskCount(),
                    NewTasks = _taskRepository.GetTaskCountByStatus(TaskStatusConstants.New),
                    InProgressTasks = _taskRepository.GetTaskCountByStatus(TaskStatusConstants.InProgress),
                    CompletedTasks = _taskRepository.GetTaskCountByStatus(TaskStatusConstants.Completed)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading dashboard statistics", ex);
            }
        }

        public List<RecentTaskDto> GetRecentTasks(int count = 5)
        {
            try
            {
                var tasks = _taskRepository.GetRecentTasks(count);

                return tasks.Select(t => new RecentTaskDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToName = t.AssignedToName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading recent tasks", ex);
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}