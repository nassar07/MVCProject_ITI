using Microsoft.EntityFrameworkCore;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MVCProject_ITI.Repositories.Implementations
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(Context context) : base(context) { }

        public async Task<IEnumerable<TaskItem>> GetUserTasks(string userId)
        {
            return await _context.TaskItems
                .Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByCategory(int categoryId, string userId)
        {
            return await _context.TaskItems
                .Where(t => t.CategoryId == categoryId && t.UserId == userId)
                .ToListAsync();
        }
    }
}
