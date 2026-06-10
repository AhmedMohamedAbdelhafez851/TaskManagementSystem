using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskManagementSystem.Tests
{
    [TestClass]
    public class DatabaseIntegrationTests
    {
        [TestMethod]
        public void DatabaseConnection_ShouldBeSuccessful()
        {
            // Arrange
            string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

            // Act & Assert
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Assert.IsTrue(conn.State == System.Data.ConnectionState.Open, "Database connection should be successful");
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Database connection failed: {ex.Message}");
                }
            }
        }

        [TestMethod]
        public void UsersTable_ShouldExistAndHaveData()
        {
            // Arrange
            string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

            // Act
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Users WHERE IsActive = 1";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    int count = (int)cmd.ExecuteScalar();

                    // Assert
                    Assert.IsTrue(count > 0, "Users table should have at least one active user");
                }
            }
        }

        [TestMethod]
        public void TasksTable_ShouldExist()
        {
            // Arrange
            string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

            // Act
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Tasks";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    int count = (int)cmd.ExecuteScalar();

                    // Assert
                    Assert.IsTrue(count >= 0, "Tasks table should be accessible");
                }
            }
        }
    }
}