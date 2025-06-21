using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Implementations;

namespace MVCProject_ITI.Controllers;

public class TaskItemController : Controller
{
    private readonly TaskRepository _taskRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public TaskItemController(TaskRepository TaskRepository)
    {
        _taskRepository = TaskRepository;
    }
    //access for admin/user
    [HttpGet]
    public async Task<IActionResult> DisplayAll()
    {
        var tasks = await _taskRepository.GetAll();
        return View(tasks);
    }
    //access for admin/user
    public async Task<IActionResult> CreateTask()
    {
        return View();
    }
    //access for admin/user
    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskItem task)
    {
        await _taskRepository.Add(task);
        await _taskRepository.SaveChanges();
        return RedirectToAction("DisplayAll");
    }
    //access for admin/user
    [HttpDelete("{ID}")]
    public async Task<IActionResult> Delete(int ID)
    {
        _taskRepository.Delete(ID);
        return RedirectToAction("DisplayAll");
    }
    //access for admin/user
    [HttpPut("{ID}")]
    public async Task<IActionResult> UpdateTask(int ID, TaskItem task)
    {
        TaskItem res = await _taskRepository.GetById(ID);

        res.IsCompleted = task.IsCompleted;
        res.Title = task.Title;
        res.Category = task.Category;
        res.Description = task.Description;
        res.DueDate = task.DueDate;
        res.User = task.User;

        await _taskRepository.Add(task);
        await _taskRepository.SaveChanges();
        return RedirectToAction("DisplayAll");
    }
}
