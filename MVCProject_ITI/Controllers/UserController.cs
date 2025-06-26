using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Implementations;
using MVCProject_ITI.Repositories.Interfaces;
using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Controllers;


public class UserController : Controller
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly Repository<Category> _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IRepository<TaskItem> TaskRepository, Repository<Category> CategoryRepository, UserManager<ApplicationUser> userManager)
    {
        _taskRepository = TaskRepository;
        _categoryRepository = CategoryRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> OrderedTasks()
    {
        var UserId = _userManager.GetUserId(User);

        var tasks = await _taskRepository.GetAll();

        var CertainUser = tasks.Where(t => t.UserId == UserId).ToList();

        return View(CertainUser);
    }
    
    public async Task<IActionResult> CreateTaskForm()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var categories = await _categoryRepository.GetAll();

        var model = new TaskViewModel
        {
            categories = categories.ToList(),
            SelectedUserId = currentUser?.Id
        };

        if (!model.categories.Any())
        {
            ModelState.AddModelError("", "No categories found.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SaveCreatedTask(TaskViewModel task)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (ModelState.IsValid)
        {
            var Task = new TaskItem
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                UserId = task.SelectedUserId,
                CategoryId = task.SelectedCategoryId
            };
            _taskRepository.Add(Task);
            await _taskRepository.SaveChanges();
            return RedirectToAction("OrderedTasks");

        }
        ModelState.AddModelError("", "Task cannot be null.");

        return View("CreateTaskForm", task);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int ID)
    {
        var task = await _taskRepository.GetById(ID);

        _taskRepository.Delete(task);
        await _taskRepository.SaveChanges();
        return RedirectToAction("OrderedTasks");
    }

    public async Task<IActionResult> UpdateTaskForm(int id)
    {
        var task = await _taskRepository.GetById(id);
        var categories = await _categoryRepository.GetAll();

        if (task == null)
            return NotFound();

        ViewBag.Categories = new SelectList(categories, "Id", "Name", task.CategoryId);

        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTask(int ID, TaskItem task)
    {
        var res = await _taskRepository.GetById(ID);

        res.Title = task.Title;
        res.Description = task.Description;
        res.IsCompleted = task.IsCompleted;
        res.DueDate = task.DueDate;
        res.CategoryId = task.CategoryId;

        await _taskRepository.SaveChanges();
        return RedirectToAction("OrderedTasks");
    }

}
