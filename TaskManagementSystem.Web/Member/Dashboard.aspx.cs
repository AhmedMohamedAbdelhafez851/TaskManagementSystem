using System;
using TaskManagementSystem.BLL.Abstraction;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Web.Utils;

namespace TaskManagementSystem.Web.Member
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private IMemberService _memberService;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login.aspx", false);
                return;
            }

            if (Session["UserRole"]?.ToString() != "Member")
            {
                Response.Redirect("~/Shared/AccessDenied.aspx", false);
                return;
            }

            try
            {
                _memberService = new MemberService();

                // Set welcome message
                if (Session["UserFullName"] != null)
                {
                    litUserName.Text = Session["UserFullName"].ToString();
                }

                if (!IsPostBack)
                {
                    LoadStatistics();
                    LoadTasks();
                    LoadNotifications();
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Dashboard Page_Load Error", ex);
                ShowError("An error occurred loading the dashboard.");
            }
        }

        private void LoadStatistics()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var stats = _memberService.GetMemberStatistics(userId);

                litTotalTasks.Text = stats.TotalTasks.ToString();
                litNewTasks.Text = stats.NewTasks.ToString();
                litInProgressTasks.Text = stats.InProgressTasks.ToString();
                litCompletedTasks.Text = stats.CompletedTasks.ToString();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("LoadStatistics Error", ex);
                SetDefaultStatistics();
            }
        }

        private void LoadTasks()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var tasks = _memberService.GetMemberTasks(userId);

                if (tasks != null && tasks.Count > 0)
                {
                    rptTasks.DataSource = tasks;
                    rptTasks.DataBind();
                    litTaskCount.Text = tasks.Count.ToString();
                    phEmpty.Visible = false;
                }
                else
                {
                    phEmpty.Visible = true;
                    litTaskCount.Text = "0";
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("LoadTasks Error", ex);
                phEmpty.Visible = true;
                ShowError("Error loading tasks.");
            }
        }

        private void LoadNotifications()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var overdueTasks = _memberService.GetOverdueNotifications(userId);

                if (overdueTasks != null && overdueTasks.Count > 0)
                {
                    pnlNotification.Visible = true;
                    rptOverdueTasks.DataSource = overdueTasks;
                    rptOverdueTasks.DataBind();
                }
                else
                {
                    pnlNotification.Visible = false;
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("LoadNotifications Error", ex);
                pnlNotification.Visible = false;
            }
        }

        private void SetDefaultStatistics()
        {
            litTotalTasks.Text = "0";
            litNewTasks.Text = "0";
            litInProgressTasks.Text = "0";
            litCompletedTasks.Text = "0";
        }

        private void ShowError(string message)
        {
            // You can add a label to show error if needed
            FileLogger.LogInfo(message);
        }

        protected string GetBadgeClass(string status)
        {
            if (status == "New") return "new";
            if (status == "InProgress") return "progress";
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