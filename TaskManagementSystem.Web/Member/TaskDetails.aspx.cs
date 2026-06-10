using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace TaskManagementSystem.Web.Member
{
    public partial class TaskDetails : Page
    {
        private int _taskId;
        private int _currentUserId;
        private string _connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Security: Check if user is logged in
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Account/Login.aspx", false);
                    return;
                }

                // Security: Check if user is Member
                string userRole = Session["UserRole"]?.ToString();
                if (userRole != "Member")
                {
                    Response.Redirect("~/Shared/AccessDenied.aspx", false);
                    return;
                }

                _connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;
                _currentUserId = Convert.ToInt32(Session["UserId"]);

                // Get Task ID from URL
                if (!int.TryParse(Request.QueryString["id"], out _taskId))
                {
                    Response.Redirect("Dashboard.aspx", false);
                    return;
                }

                // SECURITY: Verify this task belongs to the current member
                if (!IsTaskAssignedToCurrentUser())
                {
                    Response.Redirect("~/Shared/AccessDenied.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    LoadTaskDetails();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Page_Load Error: " + ex.Message);
                ShowError("An error occurred while loading the page.");
            }
        }

        // Security check - prevents member from viewing other members' tasks
        private bool IsTaskAssignedToCurrentUser()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM Tasks WHERE TaskId = @TaskId AND AssignedToUserId = @UserId AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", _taskId);
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("IsTaskAssignedToCurrentUser Error: " + ex.Message);
                return false;
            }
        }

        private void LoadTaskDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                        SELECT t.TaskId, t.Title, t.Description, t.Status,
                               t.CreatedDate, t.AssignedDate, t.AttachmentPath,
                               u.FullName AS AssignedToName
                        FROM Tasks t
                        INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                        WHERE t.TaskId = @TaskId AND t.IsDeleted = 0";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", _taskId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                litTaskId.Text = reader["TaskId"].ToString();
                                litTitle.Text = reader["Title"].ToString();
                                litDescription.Text = reader["Description"]?.ToString() ?? "No description provided.";
                                ddlStatus.SelectedValue = reader["Status"].ToString();
                                litAssignedTo.Text = reader["AssignedToName"].ToString();
                                litAssignedDate.Text = Convert.ToDateTime(reader["AssignedDate"]).ToString("yyyy-MM-dd HH:mm");
                                litCreatedDate.Text = Convert.ToDateTime(reader["CreatedDate"]).ToString("yyyy-MM-dd HH:mm");

                                string attachmentPath = reader["AttachmentPath"]?.ToString();
                                if (!string.IsNullOrEmpty(attachmentPath))
                                {
                                    lnkAttachment.NavigateUrl = attachmentPath;
                                    lnkAttachment.Visible = true;
                                    lblNoAttachment.Visible = false;
                                }
                                else
                                {
                                    lnkAttachment.Visible = false;
                                    lblNoAttachment.Visible = true;
                                }
                            }
                            else
                            {
                                Response.Redirect("Dashboard.aspx", false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadTaskDetails Error: " + ex.Message);
                ShowError("Error loading task details. Please try again.");
            }
        }

        // Member can ONLY change task status (not title/description/assigned to)
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE Tasks 
                                  SET Status = @Status, 
                                      LastModifiedDate = GETDATE()
                                  WHERE TaskId = @TaskId AND AssignedToUserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", _taskId);
                        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            ShowSuccess("Status updated successfully!");
                            LoadTaskDetails(); // Reload to show updated status
                        }
                        else
                        {
                            ShowError("Failed to update status. Please try again.");
                            LoadTaskDetails(); // Reload to reset dropdown
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ddlStatus_SelectedIndexChanged Error: " + ex.Message);
                ShowError("Error updating status: " + ex.Message);
                LoadTaskDetails();
            }
        }

        private void ShowSuccess(string message)
        {
            pnlError.Visible = false;
            pnlSuccess.Visible = true;
            lblSuccess.Text = message;

            // Auto-hide after 3 seconds
            ClientScript.RegisterStartupScript(this.GetType(), "hideSuccess",
                "setTimeout(function(){ var elem = document.getElementById('" + pnlSuccess.ClientID + "'); if(elem) elem.style.display='none'; }, 3000);", true);
        }

        private void ShowError(string message)
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = true;
            lblError.Text = message;

            // Auto-hide after 3 seconds
            ClientScript.RegisterStartupScript(this.GetType(), "hideError",
                "setTimeout(function(){ var elem = document.getElementById('" + pnlError.ClientID + "'); if(elem) elem.style.display='none'; }, 3000);", true);
        }
    }
}