using System.Collections.Generic;

namespace TaskManagementSystem.BLL.DTOs
{
    public class TaskSearchResultDto
    {
        public List<TaskDto> Tasks { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
    }
}