using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Implementations;
using MVCProject_ITI.Repositories.Interfaces;
using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Controllers;

[Authorize(Roles = "ADMIN")]
public class AdminController : Controller
{
    private readonly Repository<TaskItem> _taskRepository;
    private readonly Repository<Category> _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(Repository<TaskItem> TaskRepository, Repository<Category> CategoryRepository, UserManager<ApplicationUser> userManager)
    {
        _taskRepository = TaskRepository;
        _categoryRepository = CategoryRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> PromoteToAdmin (string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, "ADMIN");
        }
        return RedirectToAction("UsersList");
    }

    public async Task<IActionResult> UsersList()
    {
        var allUsers = await _userManager.Users.ToListAsync();

        var normalUsers = new List<ApplicationUser>();

        foreach (var user in allUsers)
        {
            if (!await _userManager.IsInRoleAsync(user, "ADMIN"))
            {
                normalUsers.Add(user);
            }
        }

        return View(normalUsers);
    }

    public async Task<IActionResult> allTasks()
    {
        var currentUserId = _userManager.GetUserId(User);

        // جلب كل التاسكات مع الكاتيجوري المرتبطة بيها
        var tasksWithCategories = await _taskRepository.GetAllWithInclude("Category");

        // فلترة التاسكات اللي الكاتيجوري بتاعها تخص اليوزر الحالي
        var myTasks = tasksWithCategories
            .Where(t => t.Category.UserId == currentUserId)
            .ToList();

        return View(myTasks);
    }



    public async Task<IActionResult> CreateTaskForm()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        List<ApplicationUser> users = await _userManager.Users.ToListAsync();

        // تحميل كل الكاتيجوريز ثم فلترة الخاصة باليوزر الحالي فقط
        var allCategories = await _categoryRepository.GetAll();
        var userCategories = allCategories
            .Where(c => c.UserId == currentUser.Id)
            .ToList();

        var model = new TaskViewModel
        {
            Users = users,
            categories = userCategories,
            SelectedUserId = currentUser.Id
        };

        if (!users.Any())
        {
            ModelState.AddModelError("", "No users found.");
        }

        if (!userCategories.Any())
        {
            ModelState.AddModelError("", "No categories found.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskViewModel task, IFormCollection form)
    {
        if (ModelState.IsValid)
        {
            var Task = new TaskItem
            {
                Title = task.Title!,
                Description = task.Description!,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                UserId = task.SelectedUserId!,
                CategoryId = task.SelectedCategoryId
            };

            _taskRepository.Add(Task);
            await _taskRepository.SaveChanges();

            return RedirectToAction("allTasks");
        }

        ModelState.AddModelError("", "Task cannot be null.");

        // إعادة تحميل البيانات للفورم عند إعادة عرضها (بعد فشل الفاليديشن)
        var currentUser = await _userManager.GetUserAsync(User);

        task.Users = await _userManager.Users.ToListAsync();

        var allCategories = await _categoryRepository.GetAll();
        task.categories = allCategories
            .Where(c => c.UserId == currentUser.Id)
            .ToList();

        return View("CreateTaskForm", task);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int ID)
    {
        var task = await _taskRepository.GetById(ID);

        _taskRepository.Delete(task);
        await _taskRepository.SaveChanges();
        return RedirectToAction("allTasks");
    }

    public async Task<IActionResult> UpdateTaskForm(int id)
    {
        var task = await _taskRepository.GetById(id);
        var users = await _userManager.Users.ToListAsync();
        var categories = await _categoryRepository.GetAll();

        if (task == null)
            return NotFound();

        ViewBag.Users = new SelectList(users, "Id", "UserName", task.UserId);
        ViewBag.Categories = new SelectList(categories, "Id", "Name", task.CategoryId);

        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTask(int ID, TaskItem task)
    {
        var res = await _taskRepository.GetById(ID);

        if (res == null)
            return NotFound();

        res.Title = task.Title;
        res.Description = task.Description;
        res.IsCompleted = task.IsCompleted;
        res.DueDate = task.DueDate;
        res.CategoryId = task.CategoryId;
        res.UserId = task.UserId;

        await _taskRepository.SaveChanges();
        return RedirectToAction("allTasks");
    }
}


