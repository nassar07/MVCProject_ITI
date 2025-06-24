using MVCProject_ITI.ViewModel;

namespace MVCProject_ITI.Repositories.Interfaces;
public interface IUserRepository
{
    Task<bool> RegisterUserAsync(RegisterViewModel model);
    Task<bool> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
}