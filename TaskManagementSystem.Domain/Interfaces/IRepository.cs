using System.Collections.Generic;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}