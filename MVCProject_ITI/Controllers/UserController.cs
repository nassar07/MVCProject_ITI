using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;
using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Controllers;


public class UserController : Controller
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IRepository<TaskItem> TaskRepository, UserManager<ApplicationUser> userManager)
    {
        _taskRepository = TaskRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> OrderedTasks()
    {
        var UserId = _userManager.GetUserId(User);

        var tasks = await _taskRepository.GetAll();

        var CertainUser = tasks.Where(t => t.UserId == UserId).ToList();

        return View("OrderedTasks", CertainUser);
    }
    
    public async Task<IActionResult> CreateTaskForm()
    {
        ViewBag.Categories = await _taskRepository.GetAll();
        return View("CreateTaskForm");
    }
    
    public async Task<IActionResult> SaveCreatedTask(TaskViewModel task)
    {
        _taskRepository.Add(new TaskItem
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CategoryId = task.Category.Id,
            UserId = _userManager.GetUserId(User)
        });
        await _taskRepository.SaveChanges();
        return RedirectToAction("OrderedTasks");

    }

    public async Task<IActionResult> Delete(int ID)
    {
        var UserId = _userManager.GetUserId(User);
        var task = await _taskRepository.GetById(ID);

        if (task.UserId == UserId)
        { 
            _taskRepository.Delete(ID);
            return RedirectToAction("OrderedTasks");
        }
        return BadRequest();
    }

    public async Task<IActionResult> UpdateTask(int ID, TaskItem task)
    {
        var res = await _taskRepository.GetById(ID);

        res.Title = task.Title;
        res.Description = task.Description;
        res.IsCompleted = task.IsCompleted;
        res.DueDate = task.DueDate;
        res.CategoryId = task.CategoryId;

         _taskRepository.Update(task);
        await _taskRepository.SaveChanges();
        return RedirectToAction("OrderedTasks");
    }
}
