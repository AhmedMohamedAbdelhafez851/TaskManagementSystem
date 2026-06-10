using System;

namespace TaskManagementSystem.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system (Admin or Member)
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? CreatedByUserId { get; set; }

        // Business logic properties
        public bool IsAdmin => Role == "Admin";
        public bool IsMember => Role == "Member";
    }
}