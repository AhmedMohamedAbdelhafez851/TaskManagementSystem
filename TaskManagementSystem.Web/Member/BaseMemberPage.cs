using System;
using System.Web.UI;

namespace TaskManagementSystem.Web.Member
{
    public abstract class BaseMemberPage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

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
        }

        protected int CurrentUserId => Convert.ToInt32(Session["UserId"]);
        protected string CurrentUserFullName => Session["UserFullName"]?.ToString();
    }
}