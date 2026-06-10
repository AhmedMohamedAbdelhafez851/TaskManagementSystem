namespace TaskManagementSystem.Domain.Constants
{
    /// <summary>
    /// Centralized constants for Task Status values
    /// Prevents hard-coded strings throughout the application
    /// </summary>
    public static class TaskStatusConstants
    {
        public const string New = "New";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";

        public static readonly string[] All = { New, InProgress, Completed };

        public static string GetDisplayName(string status)
        {
            return status switch
            {
                New => "New",
                InProgress => "In Progress",
                Completed => "Completed",
                _ => status
            };
        }

        public static bool IsValid(string status)
        {
            return status == New || status == InProgress || status == Completed;
        }
    }
}