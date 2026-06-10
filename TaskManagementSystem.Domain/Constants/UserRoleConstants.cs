namespace TaskManagementSystem.Domain.Constants
{
    /// <summary>
    /// Centralized constants for User Role values
    /// </summary>
    public static class UserRoleConstants
    {
        public const string Admin = "Admin";
        public const string Member = "Member";

        public static readonly string[] All = { Admin, Member };

        public static bool IsValid(string role)
        {
            return role == Admin || role == Member;
        }

        public static bool IsAdmin(string role) => role == Admin;
        public static bool IsMember(string role) => role == Member;
    }
}