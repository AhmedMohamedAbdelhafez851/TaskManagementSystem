using System;

namespace TaskManagementSystem.Web.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Optional: Set admin name in session if needed
            // No redirects here - let pages handle their own security
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Account/Login.aspx", false);
        }
    }
}