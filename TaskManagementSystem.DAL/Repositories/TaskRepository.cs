using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskManagementSystem.DAL.Abstraction;
using TaskManagementSystem.DAL.Helpers;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
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
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@TaskId", taskId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapTask(reader);
                }
            }
            return null;
        }

        public List<Task> GetTasksByUser(int userId)
        {
            var tasks = new List<Task>();

            string sql = @"
                SELECT t.TaskId, t.Title, t.Description, t.AssignedToUserId, t.Status,
                       t.CreatedDate, t.AssignedDate, t.AttachmentPath, t.AttachmentFileName,
                       t.CreatedByUserId, t.LastModifiedDate, t.IsDeleted,
                       u.FullName AS AssignedToName
                FROM Tasks t
                INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                WHERE t.AssignedToUserId = @UserId AND t.IsDeleted = 0
                ORDER BY t.CreatedDate DESC";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@UserId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        tasks.Add(MapTask(reader));
                }
            }
            return tasks;
        }

        public List<Task> GetOverdueTasksForUser(int userId)
        {
            var tasks = new List<Task>();

            string sql = @"
                SELECT t.TaskId, t.Title, t.Description, t.AssignedToUserId, t.Status,
                       t.CreatedDate, t.AssignedDate, t.AttachmentPath, t.AttachmentFileName,
                       t.CreatedByUserId, t.LastModifiedDate, t.IsDeleted,
                       u.FullName AS AssignedToName
                FROM Tasks t
                INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                WHERE t.AssignedToUserId = @UserId 
                    AND t.Status = 'New' 
                    AND t.IsDeleted = 0
                    AND DATEDIFF(DAY, t.AssignedDate, GETDATE()) >= 3
                ORDER BY t.AssignedDate ASC";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@UserId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        tasks.Add(MapTask(reader));
                }
            }
            return tasks;
        }

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

                using (var countCmd = DbConnectionFactory.CreateCommand(countSql, conn))
                {
                    if (assignedToUserId.HasValue) DbConnectionFactory.AddParameter(countCmd, "@AssignedToUserId", assignedToUserId.Value);
                    if (!string.IsNullOrEmpty(status)) DbConnectionFactory.AddParameter(countCmd, "@Status", status);
                    totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
                }

                // Data query with pagination
                string dataSql = @"
                    SELECT t.TaskId, t.Title, t.Description, t.AssignedToUserId, t.Status,
                           t.CreatedDate, t.AssignedDate, t.AttachmentPath, t.AttachmentFileName,
                           t.CreatedByUserId, t.LastModifiedDate, t.IsDeleted,
                           u.FullName AS AssignedToName
                    FROM Tasks t
                    INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                    WHERE t.IsDeleted = 0";

                if (assignedToUserId.HasValue) dataSql += " AND t.AssignedToUserId = @AssignedToUserId";
                if (!string.IsNullOrEmpty(status)) dataSql += " AND t.Status = @Status";

                dataSql += " ORDER BY t.CreatedDate DESC";
                dataSql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (var dataCmd = DbConnectionFactory.CreateCommand(dataSql, conn))
                {
                    if (assignedToUserId.HasValue) DbConnectionFactory.AddParameter(dataCmd, "@AssignedToUserId", assignedToUserId.Value);
                    if (!string.IsNullOrEmpty(status)) DbConnectionFactory.AddParameter(dataCmd, "@Status", status);
                    DbConnectionFactory.AddParameter(dataCmd, "@Offset", (pageNumber - 1) * pageSize);
                    DbConnectionFactory.AddParameter(dataCmd, "@PageSize", pageSize);

                    using (var reader = dataCmd.ExecuteReader())
                    {
                        while (reader.Read())
                            tasks.Add(MapTask(reader));
                    }
                }
            }
            return tasks;
        }

        public int Create(Task task)
        {
            string sql = @"
                INSERT INTO Tasks (Title, Description, AssignedToUserId, Status, 
                                   CreatedDate, AssignedDate, AttachmentPath, AttachmentFileName,
                                   CreatedByUserId, LastModifiedDate, IsDeleted)
                VALUES (@Title, @Description, @AssignedToUserId, @Status,
                        @CreatedDate, @AssignedDate, @AttachmentPath, @AttachmentFileName,
                        @CreatedByUserId, @LastModifiedDate, 0);
                SELECT SCOPE_IDENTITY();";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@Title", task.Title);
                DbConnectionFactory.AddParameter(cmd, "@Description", task.Description);
                DbConnectionFactory.AddParameter(cmd, "@AssignedToUserId", task.AssignedToUserId);
                DbConnectionFactory.AddParameter(cmd, "@Status", task.Status);
                DbConnectionFactory.AddParameter(cmd, "@CreatedDate", task.CreatedDate);
                DbConnectionFactory.AddParameter(cmd, "@AssignedDate", task.AssignedDate);
                DbConnectionFactory.AddParameter(cmd, "@AttachmentPath", task.AttachmentPath);
                DbConnectionFactory.AddParameter(cmd, "@AttachmentFileName", task.AttachmentFileName);
                DbConnectionFactory.AddParameter(cmd, "@CreatedByUserId", task.CreatedByUserId);
                DbConnectionFactory.AddParameter(cmd, "@LastModifiedDate", task.LastModifiedDate);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

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
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@TaskId", task.TaskId);
                DbConnectionFactory.AddParameter(cmd, "@Title", task.Title);
                DbConnectionFactory.AddParameter(cmd, "@Description", task.Description);
                DbConnectionFactory.AddParameter(cmd, "@AssignedToUserId", task.AssignedToUserId);
                DbConnectionFactory.AddParameter(cmd, "@Status", task.Status);
                DbConnectionFactory.AddParameter(cmd, "@AssignedDate", task.AssignedDate);
                DbConnectionFactory.AddParameter(cmd, "@AttachmentPath", task.AttachmentPath);
                DbConnectionFactory.AddParameter(cmd, "@AttachmentFileName", task.AttachmentFileName);
                DbConnectionFactory.AddParameter(cmd, "@LastModifiedDate", task.LastModifiedDate);
                DbConnectionFactory.AddParameter(cmd, "@LastModifiedByUserId", task.LastModifiedByUserId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateStatus(int taskId, string status, int modifiedByUserId)
        {
            string sql = @"
                UPDATE Tasks 
                SET Status = @Status,
                    LastModifiedDate = GETDATE(),
                    LastModifiedByUserId = @ModifiedByUserId
                WHERE TaskId = @TaskId AND IsDeleted = 0";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@TaskId", taskId);
                DbConnectionFactory.AddParameter(cmd, "@Status", status);
                DbConnectionFactory.AddParameter(cmd, "@ModifiedByUserId", modifiedByUserId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<User> GetAllMembers()
        {
            var users = new List<User>();

            string sql = "SELECT UserId, UserName, FullName, Role FROM Users WHERE Role = 'Member' AND IsActive = 1 ORDER BY FullName";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        FullName = reader.GetString(2),
                        Role = reader.GetString(3)
                    });
                }
            }
            return users;
        }

        public User GetUserById(int userId)
        {
            string sql = "SELECT UserId, UserName, FullName, Role FROM Users WHERE UserId = @UserId AND IsActive = 1";

            using (var conn = DbConnectionFactory.CreateConnection())
            using (var cmd = DbConnectionFactory.CreateCommand(sql, conn))
            {
                DbConnectionFactory.AddParameter(cmd, "@UserId", userId);

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

        private Task MapTask(SqlDataReader reader)
        {
            Task task = new Task();
            task.TaskId = reader.GetInt32(0);
            task.Title = reader.GetString(1);
            task.Description = reader.IsDBNull(2) ? null : reader.GetString(2);
            task.AssignedToUserId = reader.GetInt32(3);
            task.Status = reader.GetString(4);
            task.CreatedDate = reader.GetDateTime(5);
            task.AssignedDate = reader.GetDateTime(6);
            task.AttachmentPath = reader.IsDBNull(7) ? null : reader.GetString(7);
            task.AttachmentFileName = reader.IsDBNull(8) ? null : reader.GetString(8);
            task.CreatedByUserId = reader.GetInt32(9);

            // Fixed: Line 293 - handle nullable DateTime without ternary operator
            if (reader.IsDBNull(10))
            {
                task.LastModifiedDate = null;
            }
            else
            {
                task.LastModifiedDate = reader.GetDateTime(10);
            }

            task.IsDeleted = reader.GetBoolean(11);
            task.AssignedToName = reader.GetString(12);

            return task;
        }
    }
}