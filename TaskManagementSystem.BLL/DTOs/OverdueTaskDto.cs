using System;

namespace TaskManagementSystem.BLL.DTOs
{
    public class OverdueTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime AssignedDate { get; set; }
        public int DaysOverdue => (DateTime.Now - AssignedDate).Days;
    }
}