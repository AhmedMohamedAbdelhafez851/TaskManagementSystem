using System;

namespace TaskManagementSystem.BLL.DTOs
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedToUserId { get; set; }
        public string AssignedToName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentFileName { get; set; }
        public bool CanEditDetails { get; set; }
        public bool IsOverdue { get; set; }

        // Formatted display properties
        public string FormattedCreatedDate => CreatedDate.ToString("yyyy-MM-dd HH:mm");
        public string FormattedAssignedDate => AssignedDate.ToString("yyyy-MM-dd HH:mm");
        public string StatusDisplay => Status switch
        {
            "New" => "New",
            "InProgress" => "In Progress",
            "Completed" => "Completed",
            _ => Status
        };
    }
}