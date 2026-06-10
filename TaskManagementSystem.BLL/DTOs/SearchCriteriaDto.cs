namespace TaskManagementSystem.BLL.DTOs
{
    public class SearchCriteriaDto
    {
        public int? AssignedToUserId { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "CreatedDate";
        public string SortDirection { get; set; } = "DESC";
    }
}