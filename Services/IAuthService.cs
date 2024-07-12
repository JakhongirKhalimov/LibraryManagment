using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels.AuthViewModels;

namespace LibraryManagementSystem.Services
{
    public interface IAuthService
    {
        Task<LoginModel> LoginAsync(LoginViewModel loginViewModel);
        Task<BaseResponseModel> RegisterAsync(RegisterViewModel registerViewModel);
    }
}
