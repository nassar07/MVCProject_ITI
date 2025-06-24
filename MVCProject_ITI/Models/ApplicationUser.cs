using Microsoft.AspNetCore.Identity;

namespace MVCProject_ITI.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Category> Categories { get; set; }
    public ICollection<TaskItem> Tasks { get; set; }
}
