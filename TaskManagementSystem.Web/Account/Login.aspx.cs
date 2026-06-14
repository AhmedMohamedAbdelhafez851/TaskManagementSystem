using System;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Domain.Constants;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Web.Utils;

namespace TaskManagementSystem.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null && Session["UserRole"] != null)
            {
                RedirectBasedOnRole();
            }

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
                // Create AuthService directly (no dependency injection config needed)
                var authService = new AuthService();

                User user = authService.Authenticate(username, password);

                if (user != null)
                {
                    FileLogger.LogInfo($"User role from database: {user.Role}");
                    System.Diagnostics.Debug.WriteLine($"User role: {user.Role}");

                    Session["UserId"] = user.UserId;
                    Session["UserRole"] = user.Role;
                    Session["UserFullName"] = user.FullName;
                    Session["UserName"] = user.UserName;

                    FileLogger.LogUserAction(user.UserName, "LOGIN_SUCCESS", $"Role: {user.Role}");

                    if (user.Role == UserRoleConstants.Admin)
                    {
                        Response.Redirect("~/Admin/Dashboard.aspx", false);
                    }
                    else if (user.Role == UserRoleConstants.Member)
                    {
                        Response.Redirect("~/Member/Dashboard.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("~/Shared/AccessDenied.aspx", false);
                    }
                }
                else
                {
                    FileLogger.LogWarning($"Failed login attempt for user: {username}");
                    ShowError("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"Login error for user {username}", ex);
                ShowError("An error occurred. Please try again later.");
            }
        }

        private void RedirectBasedOnRole()
        {
            string role = Session["UserRole"]?.ToString();

            if (role == UserRoleConstants.Admin)
            {
                Response.Redirect("~/Admin/Dashboard.aspx", false);
            }
            else if (role == UserRoleConstants.Member)
            {
                Response.Redirect("~/Member/Dashboard.aspx", false);
            }
            else
            {
                Response.Redirect("~/Shared/AccessDenied.aspx", false);
            }
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            lblErrorMessage.Text = message;
        }
    }
}