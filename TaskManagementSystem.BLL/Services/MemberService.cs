using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.BLL.Abstraction;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Constants;

namespace TaskManagementSystem.BLL.Services
{
    public class MemberService : IMemberService
    {
        private readonly TaskRepository _taskRepository;

        public MemberService()
        {
            _taskRepository = new TaskRepository();
        }

        public DashboardStatisticsDto GetMemberStatistics(int userId)
        {
            try
            {
                return new DashboardStatisticsDto
                {
                    TotalTasks = _taskRepository.GetTaskCountByUser(userId),
                    NewTasks = _taskRepository.GetTaskCountByUserAndStatus(userId, TaskStatusConstants.New),
                    InProgressTasks = _taskRepository.GetTaskCountByUserAndStatus(userId, TaskStatusConstants.InProgress),
                    CompletedTasks = _taskRepository.GetTaskCountByUserAndStatus(userId, TaskStatusConstants.Completed)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading member statistics: {ex.Message}", ex);
            }
        }

        public List<MemberTaskDto> GetMemberTasks(int userId)
        {
            try
            {
                var tasks = _taskRepository.GetTasksByUser(userId);

                return tasks.Select(t => new MemberTaskDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading member tasks: {ex.Message}", ex);
            }
        }

        public List<OverdueTaskDto> GetOverdueNotifications(int userId)
        {
            try
            {
                var tasks = _taskRepository.GetOverdueTasksForUser(userId);

                return tasks.Select(t => new OverdueTaskDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    AssignedDate = t.AssignedDate
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading overdue notifications: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
        }
    }
}