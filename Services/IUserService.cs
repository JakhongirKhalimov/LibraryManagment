using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.UserViewModels;

namespace LibraryManagementSystem.Services
{
    public interface IUserService
    {
        Task<BaseListModel<UserViewModel>> UserListAsync(FilterOptions filterOptions);
        Task<BaseModel<UserViewModel>> UserByIdAsync(int id);
        Task<BaseResponseModel> CreateAsync(CreateUserViewModel createUserViewModel);
        Task<BaseResponseModel> DeleteAsync(int id);
        Task<BaseResponseModel> UpdateAsync(UpdateUserViewModel updateUserViewModel);
        Task<List<Role>> RoleListAsync();
    }
}
