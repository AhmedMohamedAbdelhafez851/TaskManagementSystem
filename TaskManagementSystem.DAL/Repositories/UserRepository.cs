using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskManagementSystem.DAL.Abstraction;
using TaskManagementSystem.DAL.Helpers;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetByUsername(string username)
        {
            try
            {
                string query = @"
                    SELECT UserId, UserName, PasswordHash, PasswordSalt, FullName, Email, Role, IsActive, CreatedDate 
                    FROM Users 
                    WHERE UserName = @UserName AND IsActive = 1";

                using (var connection = DbConnectionFactory.CreateConnection())
                using (var command = DbConnectionFactory.CreateCommand(query, connection))
                {
                    DbConnectionFactory.AddParameter(command, "@UserName", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by username {username}: {ex.Message}", ex);
            }
        }

        public User GetById(int userId)
        {
            try
            {
                string query = @"
                    SELECT UserId, UserName, PasswordHash, PasswordSalt, FullName, Email, Role, IsActive, CreatedDate 
                    FROM Users 
                    WHERE UserId = @UserId AND IsActive = 1";

                using (var connection = DbConnectionFactory.CreateConnection())
                using (var command = DbConnectionFactory.CreateCommand(query, connection))
                {
                    DbConnectionFactory.AddParameter(command, "@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by ID {userId}: {ex.Message}", ex);
            }
        }

        public List<User> GetAllMembers()
        {
            var users = new List<User>();

            try
            {
                string query = @"
                    SELECT UserId, UserName, FullName, Role, Email
                    FROM Users 
                    WHERE Role = 'Member' AND IsActive = 1 
                    ORDER BY FullName";

                using (var connection = DbConnectionFactory.CreateConnection())
                using (var command = DbConnectionFactory.CreateCommand(query, connection))
                using (var reader = command.ExecuteReader())
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
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all members: {ex.Message}", ex);
            }

            return users;
        }

        public void UpdateLastLoginDate(int userId)
        {
            try
            {
                string query = "UPDATE Users SET LastLoginDate = GETDATE() WHERE UserId = @UserId";

                using (var connection = DbConnectionFactory.CreateConnection())
                using (var command = DbConnectionFactory.CreateCommand(query, connection))
                {
                    DbConnectionFactory.AddParameter(command, "@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating last login date for user {userId}: {ex.Message}", ex);
            }
        }

        private User MapUserFromReader(SqlDataReader reader)
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