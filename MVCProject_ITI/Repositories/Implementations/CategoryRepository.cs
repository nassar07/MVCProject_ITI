using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;

namespace MVCProject_ITI.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {

        public Task AddCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
