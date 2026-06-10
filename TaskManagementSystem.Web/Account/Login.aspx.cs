using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using TaskManagementSystem.Utils;

namespace TaskManagementSystem.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FileLogger.LogDebug("Login page loaded");

            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            FileLogger.LogInfo($"Login attempt for user: {username}");

            pnlError.Visible = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT UserId, UserName, FullName, Role, PasswordHash, PasswordSalt FROM Users WHERE UserName = @UserName AND IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 60;
                        cmd.Parameters.AddWithValue("@UserName", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string dbUsername = reader.GetString(1);
                                string fullName = reader.GetString(2);
                                string role = reader.GetString(3);
                                string storedHash = reader.GetString(4);
                                string storedSalt = reader.GetString(5);

                                string computedHash = ComputeHash(password, storedSalt);

                                if (computedHash == storedHash)
                                {
                                    Session["UserId"] = userId;
                                    Session["UserRole"] = role;
                                    Session["UserFullName"] = fullName;
                                    Session["UserName"] = dbUsername;

                                    FileLogger.LogUserAction(dbUsername, "LOGIN SUCCESS", $"Role: {role}");
                                    FileLogger.LogInfo($"User {dbUsername} logged in successfully");

                                    if (role == "Admin")
                                    {
                                        Response.Redirect("~/Admin/Dashboard.aspx", false);
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Member/Dashboard.aspx", false);
                                    }
                                    return;
                                }
                                else
                                {
                                    FileLogger.LogWarning($"Failed login attempt for user: {username} - Invalid password");
                                }
                            }
                            else
                            {
                                FileLogger.LogWarning($"Failed login attempt - User not found: {username}");
                            }
                        }
                    }
                }

                pnlError.Visible = true;
                lblErrorMessage.Text = "Invalid username or password. Please try again.";
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"Login error for user {username}", ex);
                pnlError.Visible = true;
                lblErrorMessage.Text = "An error occurred: " + ex.Message;
            }
        }

        private string ComputeHash(string password, string salt)
        {
            string combined = password + salt;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }
    }
}