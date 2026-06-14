namespace TaskManagementSystem.Web.Abstraction
{
    public interface IUserSession
    {
        int CurrentUserId { get; }
        string CurrentUserName { get; }
        string CurrentUserRole { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsMember { get; }
    }
}