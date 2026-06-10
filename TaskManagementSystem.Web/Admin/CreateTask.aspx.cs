using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace TaskManagementSystem.Web.Admin
{
    public partial class CreateTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("=== CreateTask Page Loaded ===");

            if (Session["UserId"] == null)
            {
                Debug.WriteLine("Session NULL - Redirecting to Login");
                Response.Redirect("~/Account/Login.aspx", false);
                return;
            }

            if (Session["UserRole"]?.ToString() != "Admin")
            {
                Debug.WriteLine("Not Admin - Redirecting to AccessDenied");
                Response.Redirect("~/Shared/AccessDenied.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                Debug.WriteLine("Loading members...");
                LoadMembers();
            }
        }

        private void LoadMembers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT UserId, FullName FROM Users WHERE Role = 'Member' AND IsActive = 1 ORDER BY FullName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlAssignedTo.Items.Clear();
                        ddlAssignedTo.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Member --", "0"));

                        while (reader.Read())
                        {
                            ddlAssignedTo.Items.Add(new System.Web.UI.WebControls.ListItem(
                                reader["FullName"].ToString(),
                                reader["UserId"].ToString()));
                            Debug.WriteLine($"Added member: {reader["FullName"].ToString()}");
                        }
                    }
                }
                Debug.WriteLine($"Total members loaded: {ddlAssignedTo.Items.Count - 1}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading members: {ex.Message}");
                ShowError("Error loading members: " + ex.Message);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("=== Create Task Button Clicked ===");

            if (!Page.IsValid)
            {
                Debug.WriteLine("Page validation failed");
                return;
            }

            try
            {
                int assignedToUserId = int.Parse(ddlAssignedTo.SelectedValue);
                int createdByUserId = Convert.ToInt32(Session["UserId"]);

                Debug.WriteLine($"AssignedToUserId: {assignedToUserId}");
                Debug.WriteLine($"CreatedByUserId: {createdByUserId}");
                Debug.WriteLine($"Title: {txtTitle.Text.Trim()}");

                string attachmentPath = null;
                string attachmentFileName = null;

                // Handle file upload
                if (fuAttachment.HasFile)
                {
                    Debug.WriteLine($"File uploaded: {fuAttachment.FileName}");
                    string extension = Path.GetExtension(fuAttachment.FileName).ToLower();
                    string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };

                    if (!allowedExtensions.Contains(extension))
                    {
                        ShowError("Invalid file type. Allowed: PDF, DOC, DOCX, JPG, PNG");
                        return;
                    }

                    if (fuAttachment.PostedFile.ContentLength > 5 * 1024 * 1024)
                    {
                        ShowError("File size cannot exceed 5MB");
                        return;
                    }

                    string uploadFolder = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + fuAttachment.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    fuAttachment.SaveAs(filePath);

                    attachmentPath = "~/Uploads/" + uniqueFileName;
                    attachmentFileName = fuAttachment.FileName;
                    Debug.WriteLine($"File saved: {attachmentPath}");
                }

                string connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database connected");

                    string sql = @"INSERT INTO Tasks (Title, Description, AssignedToUserId, Status, 
                                    CreatedDate, AssignedDate, AttachmentPath, AttachmentFileName, 
                                    CreatedByUserId, LastModifiedDate, IsDeleted)
                                  VALUES (@Title, @Description, @AssignedToUserId, 'New',
                                    @CreatedDate, @AssignedDate, @AttachmentPath, @AttachmentFileName,
                                    @CreatedByUserId, @LastModifiedDate, 0)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(txtDescription.Text) ? (object)DBNull.Value : txtDescription.Text);
                        cmd.Parameters.AddWithValue("@AssignedToUserId", assignedToUserId);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AssignedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AttachmentPath", attachmentPath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AttachmentFileName", attachmentFileName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedByUserId", createdByUserId);
                        cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"Rows affected: {rowsAffected}");

                        if (rowsAffected > 0)
                        {
                            ShowSuccess("Task created successfully!");
                            ClearForm();
                        }
                        else
                        {
                            ShowError("Failed to create task. No rows affected.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating task: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                ShowError("Error creating task: " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Cancel button clicked - Redirecting to Dashboard");
            Response.Redirect("Dashboard.aspx", false);
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            ddlAssignedTo.SelectedIndex = 0;
        }

        private void ShowSuccess(string message)
        {
            pnlError.Visible = false;
            pnlSuccess.Visible = true;
            lblSuccess.Text = message;

            // Hide success message after 5 seconds
            ClientScript.RegisterStartupScript(this.GetType(), "hideSuccess",
                "setTimeout(function(){ document.getElementById('" + pnlSuccess.ClientID + "').style.display='none'; }, 5000);", true);
        }

        private void ShowError(string message)
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = true;
            lblError.Text = message;
        }
    }
}