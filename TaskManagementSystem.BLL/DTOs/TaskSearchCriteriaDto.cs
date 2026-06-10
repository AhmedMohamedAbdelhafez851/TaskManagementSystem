namespace TaskManagementSystem.BLL.DTOs
{
    public class TaskSearchCriteriaDto
    {
        public int? AssignedToUserId { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}