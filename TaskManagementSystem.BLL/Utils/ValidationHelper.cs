using System.Collections.Generic;
using TaskManagementSystem.BLL.DTOs;

namespace TaskManagementSystem.BLL.Utils
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }

        public string GetErrorMessage()
        {
            return string.Join(", ", Errors);
        }
    }

    public static class ValidationHelper
    {
        public static ValidationResult ValidateCreateTask(CreateTaskDto dto)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.AddError("Title is required");
            }
            else if (dto.Title.Length > 200)
            {
                result.AddError("Title cannot exceed 200 characters");
            }

            if (dto.AssignedToUserId <= 0)
            {
                result.AddError("Please select a member to assign");
            }

            return result;
        }

        public static ValidationResult ValidateUpdateTask(UpdateTaskDto dto)
        {
            var result = new ValidationResult { IsValid = true };

            if (dto.TaskId <= 0)
            {
                result.AddError("Invalid Task ID");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.AddError("Title is required");
            }
            else if (dto.Title.Length > 200)
            {
                result.AddError("Title cannot exceed 200 characters");
            }

            if (dto.AssignedToUserId <= 0)
            {
                result.AddError("Please select a member to assign");
            }

            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                result.AddError("Status is required");
            }
            else if (dto.Status != "New" && dto.Status != "InProgress" && dto.Status != "Completed")
            {
                result.AddError("Invalid status value. Allowed: New, InProgress, Completed");
            }

            return result;
        }
    }
}