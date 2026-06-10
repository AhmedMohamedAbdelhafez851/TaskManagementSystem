using System;
using System.Configuration;
using System.Data.SqlClient;

namespace TaskManagementSystem.DAL.Helpers
{
    /// <summary>
    /// Factory for creating database connections
    /// Follows Factory Design Pattern
    /// </summary>
    public static class DbConnectionFactory
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["TaskManagementDB"]?.ConnectionString
            ?? throw new InvalidOperationException("Database connection string 'TaskManagementDB' not found.");

        public static SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static SqlCommand CreateCommand(string sql, SqlConnection connection)
        {
            var command = new SqlCommand(sql, connection);
            command.CommandType = System.Data.CommandType.Text;
            command.CommandTimeout = 60;
            return command;
        }

        public static void AddParameter(SqlCommand command, string name, object value)
        {
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }
    }
}