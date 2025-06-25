using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task Add(T entity);
        void Update(T entity);
        void Delete(int id);
        Task SaveChanges();
        Task Add(TaskViewModel task);
    }
}
