using System;

namespace TaskManagementSystem.BLL.DTOs
{
    public class MemberTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StatusDisplay => GetStatusDisplayName(Status);
        public string BadgeClass => GetBadgeClass(Status);

        private string GetStatusDisplayName(string status)
        {
            return status switch
            {
                "New" => "New",
                "InProgress" => "In Progress",
                "Completed" => "Completed",
                _ => status
            };
        }

        private string GetBadgeClass(string status)
        {
            return status switch
            {
                "New" => "new",
                "InProgress" => "progress",
                "Completed" => "completed",
                _ => "new"
            };
        }
    }
}