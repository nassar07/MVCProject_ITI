using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Implementations;

namespace MVCProject_ITI.Controllers;

[Authorize(Roles = "ADMIN")]
public class CategoryController : Controller
{
    private readonly Repository<Category> _CategoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryController(Repository<Category> CategoryRepository, UserManager<ApplicationUser> userManager)
    {
        _CategoryRepository = CategoryRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _CategoryRepository.GetAll();
        return View(categories);
    }

    public async Task<IActionResult> CreateCategoryForm()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        var user = await _userManager.GetUserAsync(User);
        category.UserId = user.Id;

        _CategoryRepository.Add(category);
        _CategoryRepository.SaveChanges();

        return RedirectToAction("Index"); 
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int ID)
    {
        var category = await _CategoryRepository.GetById(ID);
        
        _CategoryRepository.Delete(category);
        await _CategoryRepository.SaveChanges();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> EditCategoryForm(int ID)
    {
        var category = await _CategoryRepository.GetById(ID);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(int ID, Category category)
    {
        var Category = await _CategoryRepository.GetById(ID);
        if (category == null)
        {
            return NotFound();
        }

        Category.Name = category.Name;

        _CategoryRepository.Update(Category);
        await _CategoryRepository.SaveChanges();
        return RedirectToAction("Index");
    }
}
