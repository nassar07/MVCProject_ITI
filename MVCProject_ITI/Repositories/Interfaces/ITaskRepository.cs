using MVCProject_ITI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MVCProject_ITI.Repositories.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetUserTasks(string userId);
        Task<IEnumerable<TaskItem>> GetTasksByCategory(int categoryId, string userId);
    }
}
