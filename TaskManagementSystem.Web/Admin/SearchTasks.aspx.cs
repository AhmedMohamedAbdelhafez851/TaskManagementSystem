using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace TaskManagementSystem.Web.Admin
{
    public partial class SearchTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    LoadMembers();
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadMembers()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT UserId, FullName FROM Users WHERE Role = 'Member' AND IsActive = 1 ORDER BY FullName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlAssignedTo.Items.Clear();
                        ddlAssignedTo.Items.Add(new ListItem("-- All Members --", ""));

                        while (reader.Read())
                        {
                            ddlAssignedTo.Items.Add(new ListItem(reader["FullName"].ToString(), reader["UserId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("LoadMembers Error: " + ex.Message);
            }
        }

        private void LoadTasks()
        {
            try
            {
                string assignedTo = ddlAssignedTo.SelectedValue;
                string status = ddlStatus.SelectedValue;
                string connStr = ConfigurationManager.ConnectionStrings["TaskManagementDB"].ConnectionString;

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                        SELECT t.TaskId, t.Title, t.Status, u.FullName AS AssignedToName
                        FROM Tasks t
                        INNER JOIN Users u ON t.AssignedToUserId = u.UserId
                        WHERE t.IsDeleted = 0";

                    if (!string.IsNullOrEmpty(assignedTo))
                    {
                        sql += " AND t.AssignedToUserId = @AssignedToUserId";
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        sql += " AND t.Status = @Status";
                    }

                    sql += " ORDER BY t.CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (!string.IsNullOrEmpty(assignedTo))
                        {
                            cmd.Parameters.AddWithValue("@AssignedToUserId", assignedTo);
                        }

                        if (!string.IsNullOrEmpty(status))
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                gvTasks.DataSource = dt;
                gvTasks.DataBind();

                litResultCount.Text = dt.Rows.Count > 0 ? $"Found {dt.Rows.Count} task(s)" : "No tasks found";
                errorDiv.Style["display"] = "none";
            }
            catch (Exception ex)
            {
                ShowError("LoadTasks Error: " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTasks();
        }

        protected void gvTasks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTasks.PageIndex = e.NewPageIndex;
            LoadTasks();
        }

        private void ShowError(string message)
        {
            litError.Text = message;
            errorDiv.Style["display"] = "block";
        }

        // Helper methods for status badges
        protected string GetStatusClass(string status)
        {
            switch (status)
            {
                case "New": return "badge-new";
                case "InProgress": return "badge-progress";
                case "Completed": return "badge-completed";
                default: return "badge-new";
            }
        }

        protected string GetStatusDisplay(string status)
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