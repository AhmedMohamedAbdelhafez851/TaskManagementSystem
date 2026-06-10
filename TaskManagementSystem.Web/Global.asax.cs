using System;
using System.Web;
using TaskManagementSystem.Utils;

namespace TaskManagementSystem.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FileLogger.LogInfo("=== APPLICATION STARTED ===");
            FileLogger.ClearOldLogs(30); // Keep logs for 30 days
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["UserId"] = null;
            Session["UserRole"] = null;
            Session["UserFullName"] = null;
            Session["UserName"] = null;
            FileLogger.LogDebug("New session started");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            FileLogger.LogError("Unhandled application error", ex);

            // Use Context.Items instead of Session (Session may not be available)
            Context.Items["LastError"] = ex.Message;

            Server.ClearError();
            Response.Redirect("~/Shared/Error.aspx");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            FileLogger.LogDebug("Session ended");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            FileLogger.LogInfo("=== APPLICATION ENDED ===");
        }
    }
}