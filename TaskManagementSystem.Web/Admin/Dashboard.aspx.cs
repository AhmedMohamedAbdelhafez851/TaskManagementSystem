using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace TaskManagementSystem.Web.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string _connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Account/Login.aspx", false);
                return;
            }

            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("~/Shared/AccessDenied.aspx", false);
                return;
            }

            _connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

            // Set welcome message
            if (Session["UserFullName"] != null)
            {
                litUserName.Text = Session["UserFullName"].ToString();
            }

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
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Total Tasks
                    string totalSql = "SELECT COUNT(*) FROM Tasks WHERE IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(totalSql, conn))
                    {
                        int total = Convert.ToInt32(cmd.ExecuteScalar());
                        litTotalTasks.Text = total.ToString();
                    }

                    // New Tasks
                    string newSql = "SELECT COUNT(*) FROM Tasks WHERE Status = 'New' AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(newSql, conn))
                    {
                        int newCount = Convert.ToInt32(cmd.ExecuteScalar());
                        litNewTasks.Text = newCount.ToString();
                    }

                    // In Progress Tasks
                    string progressSql = "SELECT COUNT(*) FROM Tasks WHERE Status = 'InProgress' AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(progressSql, conn))
                    {
                        int progress = Convert.ToInt32(cmd.ExecuteScalar());
                        litInProgressTasks.Text = progress.ToString();
                    }

                    // Completed Tasks
                    string completedSql = "SELECT COUNT(*) FROM Tasks WHERE Status = 'Completed' AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(completedSql, conn))
                    {
                        int completed = Convert.ToInt32(cmd.ExecuteScalar());
                        litCompletedTasks.Text = completed.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading statistics: " + ex.Message);
                litTotalTasks.Text = "0";
                litNewTasks.Text = "0";
                litInProgressTasks.Text = "0";
                litCompletedTasks.Text = "0";
            }
        }

        private void LoadRecentTasks()
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT TOP 5 t.TaskId, t.Title, t.Status, t.CreatedDate, u.FullName AS AssignedToName
                                  FROM Tasks t
                                  INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                                  WHERE t.IsDeleted = 0
                                  ORDER BY t.CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                gvRecentTasks.DataSource = dt;
                gvRecentTasks.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading recent tasks: " + ex.Message);
            }
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
            switch (status)
            {
                case "New": return "new";
                case "InProgress": return "inprogress";
                case "Completed": return "completed";
                default: return "new";
            }
        }

        protected string GetStatusDisplayName(string status)
        {
            switch (status)
            {
                case "New": return "New";
                case "InProgress": return "In Progress";
                case "Completed": return "Completed";
                default: return status;
            }
        }
    }
}