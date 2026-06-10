using System;

namespace TaskManagementSystem.Web.Shared
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Try to get error from Context.Items first, then Session
            string errorMessage = null;

            if (Context.Items["LastError"] != null)
            {
                errorMessage = Context.Items["LastError"].ToString();
            }
            else if (Session["LastError"] != null)
            {
                errorMessage = Session["LastError"].ToString();
                Session["LastError"] = null;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                lblErrorMessage.Text = errorMessage;
            }
            else
            {
                lblErrorMessage.Text = "An unexpected error occurred. Please try again later.";
            }
        }
    }
}