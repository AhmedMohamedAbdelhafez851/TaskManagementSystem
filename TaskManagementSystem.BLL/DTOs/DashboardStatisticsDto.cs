namespace TaskManagementSystem.BLL.DTOs
{
    public class DashboardStatisticsDto
    {
        public int TotalTasks { get; set; }
        public int NewTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
    }
}