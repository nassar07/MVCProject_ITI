using MVCProject_ITI.Models;

namespace MVCProject_ITI.ViewModel
{
    public class TaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        List<Category> categories = new List<Category>();
    }
}
