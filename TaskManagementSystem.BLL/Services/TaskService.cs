using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskManagementSystem.BLL.Abstraction;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.BLL.Utils;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Constants;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;

        public TaskService()
        {
            _taskRepository = new TaskRepository();
            _userRepository = new UserRepository();
        }

        public TaskDto GetTaskById(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            return task != null ? MapToDto(task) : null;
        }

        public List<TaskDto> GetTasksByUser(int userId)
        {
            return _taskRepository.GetTasksByUser(userId).Select(MapToDto).ToList();
        }

        public List<TaskDto> GetOverdueTasksForUser(int userId)
        {
            return _taskRepository.GetOverdueTasksForUser(userId).Select(MapToDto).ToList();
        }

        public int CreateTask(CreateTaskDto dto, int createdByUserId, string uploadFolderPath)
        {
            // Validate
            var validationResult = ValidationHelper.ValidateCreateTask(dto);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.GetErrorMessage());
            }

            var task = new Task
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                AssignedToUserId = dto.AssignedToUserId,
                Status = TaskStatusConstants.New,
                CreatedDate = DateTime.Now,
                AssignedDate = DateTime.Now,
                CreatedByUserId = createdByUserId,
                LastModifiedDate = DateTime.Now
            };

            // Handle file upload
            if (dto.AttachmentContent != null && dto.AttachmentContent.Length > 0)
            {
                if (!Directory.Exists(uploadFolderPath))
                    Directory.CreateDirectory(uploadFolderPath);

                string uniqueName = $"{Guid.NewGuid()}_{dto.AttachmentFileName}";
                string filePath = Path.Combine(uploadFolderPath, uniqueName);
                File.WriteAllBytes(filePath, dto.AttachmentContent);

                task.AttachmentPath = $"~/Uploads/{uniqueName}";
                task.AttachmentFileName = dto.AttachmentFileName;
                task.AttachmentFileSize = dto.AttachmentContent.Length;
                task.AttachmentContentType = dto.AttachmentContentType;
            }

            return _taskRepository.Create(task);
        }

        public bool UpdateTask(UpdateTaskDto dto, int modifiedByUserId, string uploadFolderPath)
        {
            // Validate
            var validationResult = ValidationHelper.ValidateUpdateTask(dto);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.GetErrorMessage());
            }

            var existingTask = _taskRepository.GetById(dto.TaskId);
            if (existingTask == null)
                throw new Exception("Task not found");

            bool isAssignedToChanged = existingTask.AssignedToUserId != dto.AssignedToUserId;
            bool canEditTitleDesc = existingTask.Status == TaskStatusConstants.New;

            if (canEditTitleDesc)
            {
                existingTask.Title = dto.Title.Trim();
                existingTask.Description = dto.Description?.Trim();
            }

            existingTask.Status = dto.Status;
            existingTask.LastModifiedDate = DateTime.Now;
            existingTask.LastModifiedByUserId = modifiedByUserId;
            existingTask.AssignedToUserId = dto.AssignedToUserId;

            if (isAssignedToChanged)
                existingTask.AssignedDate = DateTime.Now;

            // Handle file upload
            if (dto.AttachmentContent != null && dto.AttachmentContent.Length > 0)
            {
                // Delete old file
                if (!string.IsNullOrEmpty(existingTask.AttachmentPath))
                {
                    string oldPath = Path.Combine(uploadFolderPath, Path.GetFileName(existingTask.AttachmentPath));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                string uniqueName = $"{Guid.NewGuid()}_{dto.AttachmentFileName}";
                string filePath = Path.Combine(uploadFolderPath, uniqueName);
                File.WriteAllBytes(filePath, dto.AttachmentContent);

                existingTask.AttachmentPath = $"~/Uploads/{uniqueName}";
                existingTask.AttachmentFileName = dto.AttachmentFileName;
                existingTask.AttachmentFileSize = dto.AttachmentContent.Length;
                existingTask.AttachmentContentType = dto.AttachmentContentType;
            }

            return _taskRepository.Update(existingTask);
        }

        public bool UpdateTaskStatus(int taskId, string status, int modifiedByUserId)
        {
            if (!TaskStatusConstants.IsValid(status))
                throw new ArgumentException($"Invalid status value: {status}", nameof(status));

            return _taskRepository.UpdateStatus(taskId, status, modifiedByUserId);
        }

        public SearchResultDto SearchTasks(SearchCriteriaDto criteria)
        {
            var tasks = _taskRepository.SearchTasks(
                criteria.AssignedToUserId,
                criteria.Status,
                criteria.PageNumber,
                criteria.PageSize,
                out int totalCount);

            return new SearchResultDto
            {
                Tasks = tasks.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize
            };
        }

        public List<UserDto> GetAllMembers()
        {
            return _userRepository.GetAllMembers().Select(m => new UserDto
            {
                UserId = m.UserId,
                UserName = m.UserName,
                FullName = m.FullName,
                Email = m.Email,
                Role = m.Role
            }).ToList();
        }

        public bool CanUserAccessTask(int userId, int taskId, string userRole)
        {
            if (userRole == UserRoleConstants.Admin)
                return true;

            var task = _taskRepository.GetById(taskId);
            return task != null && task.AssignedToUserId == userId;
        }

        public UserDto GetUserById(int userId)
        {
            var user = _userRepository.GetById(userId);
            return user != null ? new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            } : null;
        }

        private TaskDto MapToDto(Task task)
        {
            return new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToName = task.AssignedToName,
                Status = task.Status,
                CreatedDate = task.CreatedDate,
                AssignedDate = task.AssignedDate,
                AttachmentPath = task.AttachmentPath,
                AttachmentFileName = task.AttachmentFileName,
                CanEditDetails = task.CanEditDetails,
                IsOverdue = task.IsOverdue
            };
        }
    }
}