using TaskManagementSystem.DAL.Repositories;

namespace TaskManagementSystem.Web.App_Start
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterDependencies()
        {
            // Configuration for DI container (if using one)
        }

        public static TaskRepository CreateTaskRepository()
        {
            return new TaskRepository();
        }

        public static UserRepository CreateUserRepository()
        {
            return new UserRepository();
        }
    }
}