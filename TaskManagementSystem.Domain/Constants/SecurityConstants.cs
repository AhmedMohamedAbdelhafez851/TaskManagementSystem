namespace TaskManagementSystem.Domain.Constants
{
    public static class SecurityConstants
    {
        public const int MaxFailedLoginAttempts = 5;
        public const int AccountLockoutMinutes = 15;
        public const int SessionTimeoutMinutes = 60;
        public const int RememberMeDays = 30;
    }
}