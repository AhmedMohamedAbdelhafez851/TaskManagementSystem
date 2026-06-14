using System;

namespace TaskManagementSystem.BLL.DTOs
{
    public class RecentTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AssignedToName { get; set; }

        public string StatusDisplay => GetStatusDisplay(Status);
        public string BadgeClass => GetBadgeClass(Status);

        private string GetStatusDisplay(string status)
        {
            if (status == "New") return "New";
            if (status == "InProgress") return "In Progress";
            if (status == "Completed") return "Completed";
            return status;
        }

        private string GetBadgeClass(string status)
        {
            if (status == "New") return "new";
            if (status == "InProgress") return "inprogress";
            if (status == "Completed") return "completed";
            return "new";
        }
    }
}