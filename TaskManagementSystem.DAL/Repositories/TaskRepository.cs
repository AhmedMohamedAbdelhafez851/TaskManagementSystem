using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskManagementSystem.DAL.Helpers;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Repositories
{
    public class TaskRepository
    {
        // Get task by ID
        public Task GetById(int taskId)
        {
            string sql = @"
                SELECT t.TaskId, t.Title, t.Description, t.AssignedToUserId, t.Status,
                       t.CreatedDate, t.AssignedDate, t.AttachmentPath, t.AttachmentFileName,
                       t.CreatedByUserId, t.LastModifiedDate, t.IsDeleted,
                       u.FullName AS AssignedToName
                FROM Tasks t
                INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                WHERE t.TaskId = @TaskId AND t.IsDeleted = 0";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Task
                        {
                            TaskId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            AssignedToUserId = reader.GetInt32(3),
                            Status = reader.GetString(4),
                            CreatedDate = reader.GetDateTime(5),
                            AssignedDate = reader.GetDateTime(6),
                            AttachmentPath = reader.IsDBNull(7) ? null : reader.GetString(7),
                            AttachmentFileName = reader.IsDBNull(8) ? null : reader.GetString(8),
                            CreatedByUserId = reader.GetInt32(9),
                            LastModifiedDate = reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10),
                            IsDeleted = reader.GetBoolean(11),
                            AssignedToName = reader.GetString(12)
                        };
                    }
                }
            }
            return null;
        }

        // Create new task
        public int Create(Task task)
        {
            string sql = @"
                INSERT INTO Tasks (Title, Description, AssignedToUserId, Status, 
                                   CreatedDate, AssignedDate, AttachmentPath, AttachmentFileName,
                                   CreatedByUserId, LastModifiedDate, IsDeleted)
                VALUES (@Title, @Description, @AssignedToUserId, 'New',
                        @CreatedDate, @AssignedDate, @AttachmentPath, @AttachmentFileName,
                        @CreatedByUserId, @LastModifiedDate, 0);
                SELECT SCOPE_IDENTITY();";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AssignedToUserId", task.AssignedToUserId);
                cmd.Parameters.AddWithValue("@CreatedDate", task.CreatedDate);
                cmd.Parameters.AddWithValue("@AssignedDate", task.AssignedDate);
                cmd.Parameters.AddWithValue("@AttachmentPath", task.AttachmentPath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AttachmentFileName", task.AttachmentFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedByUserId", task.CreatedByUserId);
                cmd.Parameters.AddWithValue("@LastModifiedDate", task.LastModifiedDate);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Update task
        public bool Update(Task task)
        {
            string sql = @"
                UPDATE Tasks 
                SET Title = @Title,
                    Description = @Description,
                    AssignedToUserId = @AssignedToUserId,
                    Status = @Status,
                    AssignedDate = @AssignedDate,
                    AttachmentPath = @AttachmentPath,
                    AttachmentFileName = @AttachmentFileName,
                    LastModifiedDate = @LastModifiedDate,
                    LastModifiedByUserId = @LastModifiedByUserId
                WHERE TaskId = @TaskId AND IsDeleted = 0";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TaskId", task.TaskId);
                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AssignedToUserId", task.AssignedToUserId);
                cmd.Parameters.AddWithValue("@Status", task.Status);
                cmd.Parameters.AddWithValue("@AssignedDate", task.AssignedDate);
                cmd.Parameters.AddWithValue("@AttachmentPath", task.AttachmentPath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AttachmentFileName", task.AttachmentFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LastModifiedDate", task.LastModifiedDate);
                cmd.Parameters.AddWithValue("@LastModifiedByUserId", task.LastModifiedByUserId ?? (object)DBNull.Value);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Update task status only
        public bool UpdateStatus(int taskId, string status, int modifiedByUserId)
        {
            string sql = @"
                UPDATE Tasks 
                SET Status = @Status,
                    LastModifiedDate = GETDATE(),
                    LastModifiedByUserId = @ModifiedByUserId
                WHERE TaskId = @TaskId AND IsDeleted = 0";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ModifiedByUserId", modifiedByUserId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Search tasks with filters
        public List<Task> SearchTasks(int? assignedToUserId, string status, int pageNumber, int pageSize, out int totalCount)
        {
            var tasks = new List<Task>();
            totalCount = 0;

            using (var conn = DbConnectionFactory.CreateConnection())
            {
                // Count query
                string countSql = "SELECT COUNT(*) FROM Tasks WHERE IsDeleted = 0";
                if (assignedToUserId.HasValue) countSql += " AND AssignedToUserId = @AssignedToUserId";
                if (!string.IsNullOrEmpty(status)) countSql += " AND Status = @Status";

                using (var countCmd = new SqlCommand(countSql, conn))
                {
                    if (assignedToUserId.HasValue) countCmd.Parameters.AddWithValue("@AssignedToUserId", assignedToUserId.Value);
                    if (!string.IsNullOrEmpty(status)) countCmd.Parameters.AddWithValue("@Status", status);
                    totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
                }

                // Data query
                string dataSql = @"
                    SELECT t.TaskId, t.Title, t.Status, u.FullName AS AssignedToName
                    FROM Tasks t
                    INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                    WHERE t.IsDeleted = 0";

                if (assignedToUserId.HasValue) dataSql += " AND t.AssignedToUserId = @AssignedToUserId";
                if (!string.IsNullOrEmpty(status)) dataSql += " AND t.Status = @Status";

                dataSql += " ORDER BY t.CreatedDate DESC";
                dataSql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (var dataCmd = new SqlCommand(dataSql, conn))
                {
                    if (assignedToUserId.HasValue) dataCmd.Parameters.AddWithValue("@AssignedToUserId", assignedToUserId.Value);
                    if (!string.IsNullOrEmpty(status)) dataCmd.Parameters.AddWithValue("@Status", status);
                    dataCmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    dataCmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = dataCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                TaskId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Status = reader.GetString(2),
                                AssignedToName = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return tasks;
        }

        // Get tasks by user (for Member Dashboard)
        public List<Task> GetTasksByUser(int userId)
        {
            var tasks = new List<Task>();
            string sql = @"
                SELECT TaskId, Title, Status, CreatedDate
                FROM Tasks
                WHERE AssignedToUserId = @UserId AND IsDeleted = 0
                ORDER BY CreatedDate DESC";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            TaskId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Status = reader.GetString(2),
                            CreatedDate = reader.GetDateTime(3)
                        });
                    }
                }
            }
            return tasks;
        }

        // Get overdue tasks for user
        public List<Task> GetOverdueTasksForUser(int userId)
        {
            var tasks = new List<Task>();
            string sql = @"
                SELECT TaskId, Title, AssignedDate
                FROM Tasks
                WHERE AssignedToUserId = @UserId 
                    AND Status = 'New' 
                    AND IsDeleted = 0
                    AND DATEDIFF(DAY, AssignedDate, GETDATE()) >= 3
                ORDER BY AssignedDate ASC";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            TaskId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            AssignedDate = reader.GetDateTime(2)
                        });
                    }
                }
            }
            return tasks;
        }

        // Get task count by user
        public int GetTaskCountByUser(int userId)
        {
            string sql = "SELECT COUNT(*) FROM Tasks WHERE AssignedToUserId = @UserId AND IsDeleted = 0";
            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Get task count by user and status
        public int GetTaskCountByUserAndStatus(int userId, string status)
        {
            string sql = "SELECT COUNT(*) FROM Tasks WHERE AssignedToUserId = @UserId AND Status = @Status AND IsDeleted = 0";
            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Status", status);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Get all members
        public List<User> GetAllMembers()
        {
            var users = new List<User>();
            string sql = "SELECT UserId, FullName FROM Users WHERE Role = 'Member' AND IsActive = 1 ORDER BY FullName";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = reader.GetInt32(0),
                        FullName = reader.GetString(1)
                    });
                }
            }
            return users;
        }

        // Admin methods
        public int GetTaskCount()
        {
            string sql = "SELECT COUNT(*) FROM Tasks WHERE IsDeleted = 0";
            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int GetTaskCountByStatus(string status)
        {
            string sql = "SELECT COUNT(*) FROM Tasks WHERE Status = @Status AND IsDeleted = 0";
            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Task> GetRecentTasks(int count)
        {
            var tasks = new List<Task>();
            string sql = $@"
                SELECT TOP {count} t.TaskId, t.Title, t.Status, t.CreatedDate, u.FullName AS AssignedToName
                FROM Tasks t
                INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                WHERE t.IsDeleted = 0
                ORDER BY t.CreatedDate DESC";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new Task
                    {
                        TaskId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Status = reader.GetString(2),
                        CreatedDate = reader.GetDateTime(3),
                        AssignedToName = reader.GetString(4)
                    });
                }
            }
            return tasks;
        }

        public User GetUserById(int userId)
        {
            string sql = "SELECT UserId, UserName, FullName, Role FROM Users WHERE UserId = @UserId AND IsActive = 1";
            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
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
                            FullName = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                    }
                }
            }
            return null;
        }
    }
}