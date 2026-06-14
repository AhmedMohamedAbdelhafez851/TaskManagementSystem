using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.Web.Utils;

namespace TaskManagementSystem.Web.Admin
{
    public partial class CreateTask : Page
    {
        private TaskService _taskService;
        private readonly string[] _allowedExtensions = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
        private const int MaxFileSize = 5 * 1024 * 1024;

        protected void Page_Load(object sender, EventArgs e)
        {
            FileLogger.LogDebug("CreateTask page loaded");
            _taskService = new TaskService();

            if (!IsUserAuthorized())
                return;

            if (!IsPostBack)
            {
                LoadMembers();
            }
        }

        private bool IsUserAuthorized()
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login.aspx", false);
                return false;
            }

            if (Session["UserRole"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Shared/AccessDenied.aspx", false);
                return false;
            }

            return true;
        }

        private void LoadMembers()
        {
            try
            {
                var members = _taskService.GetAllMembers();

                ddlAssignedTo.Items.Clear();
                ddlAssignedTo.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Member --", "0"));

                foreach (var member in members)
                {
                    ddlAssignedTo.Items.Add(new System.Web.UI.WebControls.ListItem(member.FullName, member.UserId.ToString()));
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Error loading members", ex);
                ShowError("Error loading members. Please refresh.");
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ClearMessages();

            if (!Page.IsValid)
            {
                ShowError("Please fill all required fields.");
                return;
            }

            try
            {
                int createdByUserId = Convert.ToInt32(Session["UserId"]);
                var dto = BuildCreateTaskDto();

                if (!ValidateFileUpload())
                    return;

                string uploadFolder = Server.MapPath("~/Uploads/");
                int taskId = _taskService.CreateTask(dto, createdByUserId, uploadFolder);

                if (taskId > 0)
                {
                    FileLogger.LogUserAction(Session["UserName"]?.ToString(), "CREATE_TASK", $"TaskId: {taskId}");
                    ShowSuccess("Task created successfully!");
                    ClearForm();
                }
                else
                {
                    ShowError("Failed to create task.");
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Error creating task", ex);
                ShowError($"Error: {ex.Message}");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx", false);
        }

        private CreateTaskDto BuildCreateTaskDto()
        {
            var dto = new CreateTaskDto
            {
                Title = txtTitle.Text.Trim(),
                Description = string.IsNullOrEmpty(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                AssignedToUserId = int.Parse(ddlAssignedTo.SelectedValue)
            };

            if (fuAttachment.HasFile)
            {
                dto.AttachmentContent = fuAttachment.FileBytes;
                dto.AttachmentFileName = fuAttachment.FileName;
                dto.AttachmentContentType = fuAttachment.PostedFile.ContentType;
            }

            return dto;
        }

        private bool ValidateFileUpload()
        {
            if (!fuAttachment.HasFile)
                return true;

            string extension = Path.GetExtension(fuAttachment.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
            {
                ShowError("Invalid file type. Allowed: PDF, DOC, DOCX, JPG, PNG");
                return false;
            }

            if (fuAttachment.PostedFile.ContentLength > MaxFileSize)
            {
                ShowError("File size cannot exceed 5MB");
                return false;
            }

            return true;
        }

        private void ClearMessages()
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = false;
        }

        private void ClearForm()
        {
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlAssignedTo.SelectedIndex = 0;
        }

        private void ShowSuccess(string message)
        {
            pnlSuccess.Visible = true;
            lblSuccess.Text = message;
            pnlError.Visible = false;

            string script = string.Format(
                "setTimeout(function(){{ document.getElementById('{0}').style.display = 'none'; }}, 5000);",
                pnlSuccess.ClientID);
            ClientScript.RegisterStartupScript(GetType(), "HideSuccess", script, true);
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            lblError.Text = message;
            pnlSuccess.Visible = false;
        }
    }
}