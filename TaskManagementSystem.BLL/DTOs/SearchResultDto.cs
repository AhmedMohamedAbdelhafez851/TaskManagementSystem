using System;
using System.Collections.Generic;

namespace TaskManagementSystem.BLL.DTOs
{
    public class SearchResultDto
    {
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}