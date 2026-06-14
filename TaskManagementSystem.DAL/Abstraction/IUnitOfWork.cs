using System;

namespace TaskManagementSystem.DAL.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        int Complete();  // Use Complete() instead of Commit()
        void Rollback();
    }
}