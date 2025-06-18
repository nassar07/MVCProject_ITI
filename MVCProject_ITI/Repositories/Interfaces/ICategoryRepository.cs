using MVCProject_ITI.Models;

namespace MVCProject_ITI.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetUserCategories(string userId);
    }

}
