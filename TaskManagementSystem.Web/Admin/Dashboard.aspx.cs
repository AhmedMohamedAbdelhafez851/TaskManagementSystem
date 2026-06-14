using System;
using TaskManagementSystem.BLL.Abstraction;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Web.Utils;

namespace TaskManagementSystem.Web.Admin
{
    public partial class Dashboard : BaseAdminPage
    {
        private IAdminService _adminService;

        protected void Page_Load(object sender, EventArgs e)
        {
            FileLogger.LogDebug("Admin Dashboard loaded");
            _adminService = new AdminService();

            litUserName.Text = CurrentUserFullName;

            if (!IsPostBack)
            {
                LoadStatistics();
                LoadRecentTasks();
            }
        }

        private void LoadStatistics()
        {
            try
            {
                var stats = _adminService.GetDashboardStatistics();

                litTotalTasks.Text = stats.TotalTasks.ToString();
                litNewTasks.Text = stats.NewTasks.ToString();
                litInProgressTasks.Text = stats.InProgressTasks.ToString();
                litCompletedTasks.Text = stats.CompletedTasks.ToString();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Error loading statistics", ex);
                SetDefaultStatistics();
            }
        }

        private void LoadRecentTasks()
        {
            try
            {
                var tasks = _adminService.GetRecentTasks(5);
                gvRecentTasks.DataSource = tasks;
                gvRecentTasks.DataBind();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Error loading recent tasks", ex);
            }
        }

        private void SetDefaultStatistics()
        {
            litTotalTasks.Text = "0";
            litNewTasks.Text = "0";
            litInProgressTasks.Text = "0";
            litCompletedTasks.Text = "0";
        }

        protected void btnCreateTask_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateTask.aspx", false);
        }

        protected void btnSearchTasks_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchTasks.aspx", false);
        }

        protected string GetBadgeClass(string status)
        {
            if (status == "New") return "new";
            if (status == "InProgress") return "inprogress";
            if (status == "Completed") return "completed";
            return "new";
        }

        protected string GetStatusDisplayName(string status)
        {
            if (status == "New") return "New";
            if (status == "InProgress") return "In Progress";
            if (status == "Completed") return "Completed";
            return status;
        }
    }
}