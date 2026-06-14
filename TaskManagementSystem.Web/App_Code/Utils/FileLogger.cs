using System;
using System.IO;
using System.Web;

namespace TaskManagementSystem.Web.Utils
{
    /// <summary>
    /// Simple file logger for the application
    /// Logs errors, warnings, and information to text files
    /// </summary>
    public static class FileLogger
    {
        private static readonly object LockObject = new object();
        private static string _logFolderPath;

        static FileLogger()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    _logFolderPath = HttpContext.Current.Server.MapPath("~/App_Data/Logs");
                }
                else
                {
                    _logFolderPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Logs";
                }

                if (!Directory.Exists(_logFolderPath))
                {
                    Directory.CreateDirectory(_logFolderPath);
                }
            }
            catch
            {
                _logFolderPath = Path.GetTempPath() + "TaskManagementLogs\\";
                if (!Directory.Exists(_logFolderPath))
                {
                    Directory.CreateDirectory(_logFolderPath);
                }
            }
        }

        public static void LogError(string message, Exception ex = null)
        {
            string logMessage = "[ERROR] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message;
            if (ex != null)
            {
                logMessage = logMessage + "\n    Exception: " + ex.Message;
                logMessage = logMessage + "\n    StackTrace: " + ex.StackTrace;
            }
            WriteToFile(logMessage);
        }

        public static void LogWarning(string message)
        {
            string logMessage = "[WARNING] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message;
            WriteToFile(logMessage);
        }

        public static void LogInfo(string message)
        {
            string logMessage = "[INFO] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message;
            WriteToFile(logMessage);
        }

        public static void LogDebug(string message)
        {
#if DEBUG
            string logMessage = "[DEBUG] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message;
            WriteToFile(logMessage);
#endif
        }

        public static void LogUserAction(string username, string action, string details = null)
        {
            string logMessage = "[USER ACTION] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - User: " + username + " - Action: " + action;
            if (!string.IsNullOrEmpty(details))
            {
                logMessage = logMessage + " - Details: " + details;
            }
            WriteToFile(logMessage, "UserActions.log");
        }

        private static void WriteToFile(string message, string fileName = null)
        {
            try
            {
                lock (LockObject)
                {
                    string logFileName = fileName ?? ("log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                    string fullPath = Path.Combine(_logFolderPath, logFileName);

                    string directory = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (StreamWriter writer = new StreamWriter(fullPath, true))
                    {
                        writer.WriteLine(message);
                        writer.WriteLine(new string('-', 80));
                    }
                }
            }
            catch
            {
                // Silently fail - logging should not break the application
            }
        }

        public static string GetLogFolderPath()
        {
            return _logFolderPath;
        }

        public static void ClearOldLogs(int daysToKeep = 30)
        {
            try
            {
                if (!Directory.Exists(_logFolderPath))
                {
                    return;
                }

                string[] files = Directory.GetFiles(_logFolderPath, "*.log");
                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);

                foreach (string file in files)
                {
                    if (File.GetCreationTime(file) < cutoffDate)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch
            {
                // Silently fail
            }
        }
    }
}