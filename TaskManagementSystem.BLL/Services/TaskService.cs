using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.DAL.Repositories;
using TaskManagementSystem.Domain.Constants;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.BLL.Services
{
    public class TaskService
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
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Task title is required");

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

            if (dto.AttachmentContent != null && dto.AttachmentContent.Length > 0)
            {
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
                Role = m.Role
            }).ToList();
        }

        public UserDto GetUserById(int userId)
        {
            var user = _userRepository.GetById(userId);
            return user != null ? new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
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