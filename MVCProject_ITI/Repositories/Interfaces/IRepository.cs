using MVCProject_ITI.Models;

namespace MVCProject_ITI.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IQueryable<T>> GetAllWithInclude(params string[] includes);
        Task<T?> GetById(int id);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChanges();
        void Add(TaskItem task);
    }
}
