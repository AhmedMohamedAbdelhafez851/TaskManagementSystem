using System;
using TaskManagementSystem.Domain.Constants;
using TaskManagementSystem.Domain.Interfaces;

namespace TaskManagementSystem.Domain.Entities
{
    public class Task : IEntity
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedToUserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentFileName { get; set; }
        public int? AttachmentFileSize { get; set; }
        public string AttachmentContentType { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public bool IsDeleted { get; set; }

        // IEntity implementation
        public int Id
        {
            get => TaskId;
            set => TaskId = value;
        }

        // Display properties (from JOIN)
        public string AssignedToName { get; set; }
        public string CreatedByName { get; set; }

        // Business logic properties
        public bool CanEditDetails => Status == TaskStatusConstants.New;
        public bool IsOverdue => Status == TaskStatusConstants.New && (DateTime.Now - AssignedDate).Days >= 3;
    }
}