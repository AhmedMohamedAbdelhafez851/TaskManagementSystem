//using System;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace TaskManagementSystem.Web
//{
//    public class BaseAdminPage : Page
//    {
//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);

//            // Check if user is logged in - NO null propagating operator
//            object userId = Session["UserId"];
//            if (userId == null)
//            {
//                HttpContext.Current.Response.Redirect("~/Account/Login.aspx", false);
//                return;
//            }

//            // Check if user is Admin
//            object userRoleObj = Session["UserRole"];
//            string userRole = userRoleObj != null ? userRoleObj.ToString() : "";
//            if (userRole != "Admin")
//            {
//                HttpContext.Current.Response.Redirect("~/Shared/AccessDenied.aspx", false);
//                return;
//            }
//        }

//        protected void ShowSuccess(string message, Panel pnlSuccess, Label lblSuccess)
//        {
//            pnlSuccess.Visible = true;
//            lblSuccess.Text = message;
//        }

//        protected void ShowError(string message, Panel pnlError, Label lblError)
//        {
//            pnlError.Visible = true;
//            lblError.Text = message;
//        }
//    }
//}