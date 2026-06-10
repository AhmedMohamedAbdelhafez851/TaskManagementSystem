using System;

namespace TaskManagementSystem.Web.MasterPages
{
    public partial class Member : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
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

            // Set user name in the navbar
            if (Session["UserFullName"] != null)
            {
                litUserName.Text = Session["UserFullName"].ToString();
            }
            else if (Session["UserName"] != null)
            {
                litUserName.Text = Session["UserName"].ToString();
            }
            else
            {
                litUserName.Text = "Member";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Account/Login.aspx", false);
        }
    }
}