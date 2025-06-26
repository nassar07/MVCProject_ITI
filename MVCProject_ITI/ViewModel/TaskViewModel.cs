using MVCProject_ITI.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCProject_ITI.ViewModel;

public class TaskViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Due date is required")]
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public List<Category>? categories { get; set; }
    public List<ApplicationUser>? Users { get; set; }

    [Required(ErrorMessage = "Please select a user")]
    public string? SelectedUserId { get; set; }

    [Required(ErrorMessage = "Please select a category")]
    public int SelectedCategoryId { get; set; }
}
