using System;
using System.Configuration;
using System.Data.SqlClient;

namespace TaskManagementSystem.DAL.Helpers
{
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

        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}