﻿namespace MVCProject_ITI.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public ICollection<TaskItem> TaskItems { get; set; }
}
