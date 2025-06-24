using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Implementations;

namespace MVCProject_ITI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly Repository<TaskItem> _taskRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(Repository<TaskItem> TaskRepository, UserManager<ApplicationUser> userManager)
    {
        _taskRepository = TaskRepository;
        _userManager = userManager;
    }
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        return RedirectToAction("UsersList");
    }

    public async Task<IActionResult> allTasks()
    {
        var tasks = await _taskRepository.GetAll();
        return View(tasks);
    }

    public async Task<IActionResult> CreateTaskForm()
    {
        var users = await _userManager.Users.ToListAsync();

        ViewBag.Users = users;

        return View();
    }

    public async Task<IActionResult> CreateTask(TaskItem task)
    {
        var Orderedtask = new TaskItem
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            UserId = task.UserId,
            CategoryId = task.CategoryId,
            IsCompleted = task.IsCompleted,
        };

        await _taskRepository.Add(Orderedtask);
        await _taskRepository.SaveChanges();
        return RedirectToAction("OrderedTasks");
    }

    public async Task<IActionResult> Delete(int ID)
    {
        _taskRepository.Delete(ID);
        return RedirectToAction("allTasks");
    }

    public async Task<IActionResult> UpdateTask(int ID, TaskItem task)
    {
        var res = await _taskRepository.GetById(ID);

        res.Title = task.Title;
        res.Description = task.Description;
        res.IsCompleted = task.IsCompleted;
        res.DueDate = task.DueDate;
        res.CategoryId = task.CategoryId;
        res.UserId = task.UserId;

        await _taskRepository.Add(task);
        await _taskRepository.SaveChanges();
        return RedirectToAction("OrderedTasks");
    }
}


