using Microsoft.AspNetCore.Identity;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;
using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> RegisterUserAsync(RegisterViewModel model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return false;

        await _userManager.AddToRoleAsync(user, "USER");
        return true;
    }

    public async Task<bool> LoginAsync(LoginViewModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

}