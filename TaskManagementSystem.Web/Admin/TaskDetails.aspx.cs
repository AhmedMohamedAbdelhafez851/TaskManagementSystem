using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace TaskManagementSystem.Web.Admin
{
    public partial class TaskDetails : Page
    {
        private int _taskId;
        private string _connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check if user is logged in
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Account/Login.aspx", false);
                    return;
                }

                // Check if user is Admin
                string userRole = Session["UserRole"]?.ToString();
                if (userRole != "Admin")
                {
                    Response.Redirect("~/Shared/AccessDenied.aspx", false);
                    return;
                }

                _connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

                // Get Task ID from URL
                if (!int.TryParse(Request.QueryString["id"], out _taskId))
                {
                    Response.Redirect("SearchTasks.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    LoadMembers();
                    LoadTaskDetails();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Page_Load Error: " + ex.Message);
                ShowError("An error occurred while loading the page.");
            }
        }

        private void LoadMembers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT UserId, FullName FROM Users WHERE Role = 'Member' AND IsActive = 1 ORDER BY FullName";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlAssignedTo.Items.Clear();
                        while (reader.Read())
                        {
                            ddlAssignedTo.Items.Add(new System.Web.UI.WebControls.ListItem(
                                reader["FullName"].ToString(), reader["UserId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadMembers Error: " + ex.Message);
                ShowError("Error loading members list.");
            }
        }

        private void LoadTaskDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT TaskId, Title, Description, AssignedToUserId, Status,
                                   CreatedDate, AssignedDate, AttachmentPath
                                  FROM Tasks WHERE TaskId = @TaskId AND IsDeleted = 0";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", _taskId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                litTaskId.Text = reader["TaskId"].ToString();
                                txtTitle.Text = reader["Title"].ToString();
                                txtDescription.Text = reader["Description"]?.ToString() ?? "";
                                ddlAssignedTo.SelectedValue = reader["AssignedToUserId"].ToString();
                                ddlStatus.SelectedValue = reader["Status"].ToString();
                                litAssignedDate.Text = Convert.ToDateTime(reader["AssignedDate"]).ToString("yyyy-MM-dd HH:mm");
                                litCreatedDate.Text = Convert.ToDateTime(reader["CreatedDate"]).ToString("yyyy-MM-dd HH:mm");

                                // RULE 1: Only New tasks can edit Title and Description
                                bool isNewTask = reader["Status"].ToString() == "New";
                                txtTitle.Enabled = isNewTask;
                                txtDescription.Enabled = isNewTask;

                                if (!isNewTask)
                                {
                                    txtTitle.CssClass = "form-control-custom bg-light";
                                    txtDescription.CssClass = "form-control-custom bg-light";
                                }

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
                                Response.Redirect("SearchTasks.aspx", false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadTaskDetails Error: " + ex.Message);
                ShowError("Error loading task details.");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int oldAssignedToUserId = 0;
                string currentStatus = "";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Get current values before update
                    string getSql = "SELECT AssignedToUserId, Status FROM Tasks WHERE TaskId = @TaskId";
                    using (SqlCommand getCmd = new SqlCommand(getSql, conn))
                    {
                        getCmd.Parameters.AddWithValue("@TaskId", _taskId);
                        using (SqlDataReader reader = getCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldAssignedToUserId = reader.GetInt32(0);
                                currentStatus = reader.GetString(1);
                            }
                        }
                    }

                    int newAssignedToUserId = int.Parse(ddlAssignedTo.SelectedValue);
                    string newStatus = ddlStatus.SelectedValue;

                    // Check if Assigned To changed (THIS IS THE ONLY THING THAT RESETS ASSIGNED DATE)
                    bool isAssignedToChanged = oldAssignedToUserId != newAssignedToUserId;

                    // Check if Title/Description can be edited (only if status is New)
                    bool canEditTitleDesc = currentStatus == "New";

                    // Build the UPDATE SQL statement
                    string updateSql = @"UPDATE Tasks 
                                        SET Status = @Status, 
                                            LastModifiedDate = @LastModifiedDate";

                    // Only update Title and Description if the task is New (RULE 1)
                    if (canEditTitleDesc)
                    {
                        updateSql += ", Title = @Title, Description = @Description";
                    }

                    // Always update AssignedToUserId
                    updateSql += ", AssignedToUserId = @AssignedToUserId";

                    // RULE 2: Reset AssignedDate ONLY if AssignedTo changed
                    if (isAssignedToChanged)
                    {
                        updateSql += ", AssignedDate = @AssignedDate";
                    }

                    updateSql += " WHERE TaskId = @TaskId";

                    using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@TaskId", _taskId);
                        updateCmd.Parameters.AddWithValue("@Status", newStatus);
                        updateCmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@AssignedToUserId", newAssignedToUserId);

                        // Only add Title/Description parameters if allowed
                        if (canEditTitleDesc)
                        {
                            updateCmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(txtDescription.Text) ? (object)DBNull.Value : txtDescription.Text);
                        }

                        // ONLY add AssignedDate parameter if AssignedTo changed
                        if (isAssignedToChanged)
                        {
                            updateCmd.Parameters.AddWithValue("@AssignedDate", DateTime.Now);
                        }

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string successMsg = "Task updated successfully!";

                            if (isAssignedToChanged)
                            {
                                successMsg += " Assigned date has been reset to current date.";
                            }

                            if (!canEditTitleDesc && (txtTitle.Enabled == false))
                            {
                                successMsg += " Note: Title and Description cannot be edited because task status is not 'New'.";
                            }

                            ShowSuccess(successMsg);
                            LoadTaskDetails(); // Reload to show updated values
                        }
                        else
                        {
                            ShowError("Failed to update task.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("btnSave_Click Error: " + ex.Message);
                ShowError("Error saving task: " + ex.Message);
            }
        }

        private void ShowSuccess(string message)
        {
            pnlError.Visible = false;
            pnlSuccess.Visible = true;
            lblSuccess.Text = message;

            ClientScript.RegisterStartupScript(this.GetType(), "hideSuccess",
                "setTimeout(function(){ var elem = document.getElementById('" + pnlSuccess.ClientID + "'); if(elem) elem.style.display='none'; }, 5000);", true);
        }

        private void ShowError(string message)
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = true;
            lblError.Text = message;

            ClientScript.RegisterStartupScript(this.GetType(), "hideError",
                "setTimeout(function(){ var elem = document.getElementById('" + pnlError.ClientID + "'); if(elem) elem.style.display='none'; }, 5000);", true);
        }
    }
}