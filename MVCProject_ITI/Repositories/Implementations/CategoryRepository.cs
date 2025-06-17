using Microsoft.EntityFrameworkCore;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;
using System;

namespace MVCProject_ITI.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(Context context) : base(context) { }

        public async Task<IEnumerable<Category>> GetUserCategories(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }

}
