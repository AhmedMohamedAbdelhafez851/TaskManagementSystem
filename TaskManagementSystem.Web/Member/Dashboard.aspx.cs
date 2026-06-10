using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaskManagementSystem.Web.MasterPages;

namespace TaskManagementSystem.Web.Member
{
    public partial class Dashboard : Page
    {
        private string _connectionString;
        private int _currentUserId;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Security Check - User must be logged in
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Account/Login.aspx", false);
                    return;
                }

                // Security Check - User must be Member
                if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Member")
                {
                    Response.Redirect("~/Shared/AccessDenied.aspx", false);
                    return;
                }

                _currentUserId = Convert.ToInt32(Session["UserId"]);
                _connectionString = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

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
                System.Diagnostics.Debug.WriteLine("Page_Load Error: " + ex.Message);
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
                    string totalSql = "SELECT COUNT(*) FROM Tasks WHERE AssignedToUserId = @UserId AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(totalSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);
                        litTotalTasks.Text = cmd.ExecuteScalar().ToString();
                    }

                    // New Tasks
                    string newSql = "SELECT COUNT(*) FROM Tasks WHERE AssignedToUserId = @UserId AND Status = 'New' AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(newSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);
                        litNewTasks.Text = cmd.ExecuteScalar().ToString();
                    }

                    // In Progress Tasks
                    string progressSql = "SELECT COUNT(*) FROM Tasks WHERE AssignedToUserId = @UserId AND Status = 'InProgress' AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(progressSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);
                        litInProgressTasks.Text = cmd.ExecuteScalar().ToString();
                    }

                    // Completed Tasks
                    string completedSql = "SELECT COUNT(*) FROM Tasks WHERE AssignedToUserId = @UserId AND Status = 'Completed' AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(completedSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);
                        litCompletedTasks.Text = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStatistics Error: " + ex.Message);
                litTotalTasks.Text = "0";
                litNewTasks.Text = "0";
                litInProgressTasks.Text = "0";
                litCompletedTasks.Text = "0";
            }
        }

        private void LoadTasks()
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT TaskId, Title, Status, CreatedDate
                                  FROM Tasks
                                  WHERE AssignedToUserId = @UserId AND IsDeleted = 0
                                  ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    rptTasks.DataSource = dt;
                    rptTasks.DataBind();
                    litTaskCount.Text = dt.Rows.Count.ToString();
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
                System.Diagnostics.Debug.WriteLine("LoadTasks Error: " + ex.Message);
                phEmpty.Visible = true;
            }
        }

        private void LoadNotifications()
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT TaskId, Title, AssignedDate
                                  FROM Tasks
                                  WHERE AssignedToUserId = @UserId 
                                    AND Status = 'New' 
                                    AND IsDeleted = 0
                                    AND DATEDIFF(DAY, AssignedDate, GETDATE()) >= 3
                                  ORDER BY AssignedDate ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", _currentUserId);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    pnlNotification.Visible = true;
                    rptOverdueTasks.DataSource = dt;
                    rptOverdueTasks.DataBind();
                }
                else
                {
                    pnlNotification.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadNotifications Error: " + ex.Message);
                pnlNotification.Visible = false;
            }
        }

        protected string GetBadgeClass(string status)
        {
            switch (status)
            {
                case "New": return "new";
                case "InProgress": return "progress";
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