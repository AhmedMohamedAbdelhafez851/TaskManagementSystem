//using System;
//using System.Web.UI;
//using System.Web;

//namespace TaskManagementSystem.Web
//{
//    public class BaseMemberPage : Page
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

//            // Check if user is Member
//            object userRoleObj = Session["UserRole"];
//            string userRole = userRoleObj != null ? userRoleObj.ToString() : "";
//            if (userRole != "Member")
//            {
//                HttpContext.Current.Response.Redirect("~/Shared/AccessDenied.aspx", false);
//                return;
//            }
//        }

//        protected int CurrentUserId
//        {
//            get
//            {
//                object userId = Session["UserId"];
//                if (userId != null)
//                {
//                    return Convert.ToInt32(userId);
//                }
//                return 0;
//            }
//        }

//        protected string CurrentUserFullName
//        {
//            get
//            {
//                object fullName = Session["UserFullName"];
//                if (fullName != null)
//                {
//                    return fullName.ToString();
//                }
//                return string.Empty;
//            }
//        }
//    }
//}