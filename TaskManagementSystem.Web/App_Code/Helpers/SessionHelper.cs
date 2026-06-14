using System;
using System.Web;

namespace TaskManagementSystem.Web
{
    public static class SessionHelper
    {
        public static int CurrentUserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                }
                return 0;
            }
        }

        public static string CurrentUserRole
        {
            get
            {
                if (HttpContext.Current.Session["UserRole"] != null)
                {
                    return HttpContext.Current.Session["UserRole"].ToString();
                }
                return string.Empty;
            }
        }

        public static string CurrentUserFullName
        {
            get
            {
                if (HttpContext.Current.Session["UserFullName"] != null)
                {
                    return HttpContext.Current.Session["UserFullName"].ToString();
                }
                return string.Empty;
            }
        }

        public static bool IsAuthenticated
        {
            get { return CurrentUserId > 0; }
        }

        public static bool IsMember
        {
            get { return CurrentUserRole == "Member"; }
        }
    }
}