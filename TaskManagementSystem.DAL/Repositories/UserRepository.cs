using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskManagementSystem.DAL.Helpers;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Repositories
{
    public class UserRepository
    {
        public User GetByUsername(string username)
        {
            string query = @"
                SELECT UserId, UserName, PasswordHash, PasswordSalt, FullName, Email, Role, IsActive, CreatedDate 
                FROM Users 
                WHERE UserName = @UserName AND IsActive = 1";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserName", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            PasswordSalt = reader.GetString(3),
                            FullName = reader.GetString(4),
                            Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Role = reader.GetString(6),
                            IsActive = reader.GetBoolean(7),
                            CreatedDate = reader.GetDateTime(8)
                        };
                    }
                }
            }
            return null;
        }

        public User GetById(int userId)
        {
            string query = @"
                SELECT UserId, UserName, PasswordHash, PasswordSalt, FullName, Email, Role, IsActive, CreatedDate 
                FROM Users 
                WHERE UserId = @UserId AND IsActive = 1";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            PasswordSalt = reader.GetString(3),
                            FullName = reader.GetString(4),
                            Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Role = reader.GetString(6),
                            IsActive = reader.GetBoolean(7),
                            CreatedDate = reader.GetDateTime(8)
                        };
                    }
                }
            }
            return null;
        }

        public List<User> GetAllMembers()
        {
            var users = new List<User>();
            string query = @"
                SELECT UserId, UserName, FullName, Role, Email
                FROM Users 
                WHERE Role = 'Member' AND IsActive = 1 
                ORDER BY FullName";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        FullName = reader.GetString(2),
                        Role = reader.GetString(3),
                        Email = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
            }
            return users;
        }

        public void UpdateLastLoginDate(int userId)
        {
            string query = "UPDATE Users SET LastLoginDate = GETDATE() WHERE UserId = @UserId";
            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}