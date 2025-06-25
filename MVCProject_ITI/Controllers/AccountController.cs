using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;
using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public IActionResult Register()
    {
        return View("Register");
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Register", model);

        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user,"User");
        return View("Login");
    }

    public IActionResult Login()
    {
        return View("Login");
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Login",model);

        bool succeeded = await _userRepository.LoginAsync(model);
        if (succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _userRepository.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}