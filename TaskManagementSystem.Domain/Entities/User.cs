using System;
using TaskManagementSystem.Domain.Interfaces;

namespace TaskManagementSystem.Domain.Entities
{
    public class User : IEntity
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

        // IEntity implementation
        public int Id
        {
            get => UserId;
            set => UserId = value;
        }

        public bool IsAdmin => Role == "Admin";
        public bool IsMember => Role == "Member";
    }
}