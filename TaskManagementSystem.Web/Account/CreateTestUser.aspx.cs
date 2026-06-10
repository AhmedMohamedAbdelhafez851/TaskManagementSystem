using System;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace TaskManagementSystem.Web.Account
{
    public partial class CreateTestUser : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCreateAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = "admin";
                string password = "Admin@123";
                string salt = GenerateSalt();
                string hash = ComputeHash(password, salt);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Delete existing admin
                    string deleteSql = "DELETE FROM Users WHERE UserName = @UserName";
                    using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.ExecuteNonQuery();
                    }

                    // Insert new admin
                    string insertSql = @"
                        INSERT INTO Users (UserName, PasswordHash, PasswordSalt, FullName, Email, Role, IsActive, CreatedDate)
                        VALUES (@UserName, @PasswordHash, @PasswordSalt, @FullName, @Email, @Role, @IsActive, @CreatedDate)";

                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", hash);
                        cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                        cmd.Parameters.AddWithValue("@FullName", "System Administrator");
                        cmd.Parameters.AddWithValue("@Email", "admin@taskmanagement.com");
                        cmd.Parameters.AddWithValue("@Role", "Admin");
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }

                ShowMessage("Admin user created successfully! Username: admin, Password: Admin@123", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, false);
            }
        }

        protected void btnCreateMembers_Click(object sender, EventArgs e)
        {
            try
            {
                string password = "Member@123";
                string salt = GenerateSalt();
                string hash = ComputeHash(password, salt);

                var members = new[]
                {
                    new { UserName = "ahmed.ali", FullName = "Ahmed Ali", Email = "ahmed@company.com" },
                    new { UserName = "sara.hassan", FullName = "Sara Hassan", Email = "sara@company.com" },
                    new { UserName = "mohamed.kamal", FullName = "Mohamed Kamal", Email = "mohamed@company.com" }
                };

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var member in members)
                    {
                        // Delete existing
                        string deleteSql = "DELETE FROM Users WHERE UserName = @UserName";
                        using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserName", member.UserName);
                            cmd.ExecuteNonQuery();
                        }

                        // Insert new member
                        string insertSql = @"
                            INSERT INTO Users (UserName, PasswordHash, PasswordSalt, FullName, Email, Role, IsActive, CreatedDate)
                            VALUES (@UserName, @PasswordHash, @PasswordSalt, @FullName, @Email, @Role, @IsActive, @CreatedDate)";

                        using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserName", member.UserName);
                            cmd.Parameters.AddWithValue("@PasswordHash", hash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@FullName", member.FullName);
                            cmd.Parameters.AddWithValue("@Email", member.Email);
                            cmd.Parameters.AddWithValue("@Role", "Member");
                            cmd.Parameters.AddWithValue("@IsActive", true);
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                ShowMessage("Member users created successfully! Password for all members: Member@123", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, false);
            }
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string ComputeHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string combined = password + salt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            if (isSuccess)
            {
                pnlMessage.CssClass = "alert alert-success";
            }
            else
            {
                pnlMessage.CssClass = "alert alert-danger";
            }
        }
    }
}