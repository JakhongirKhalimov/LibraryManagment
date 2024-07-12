using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.ViewModels.UserViewModels
{
    public class CreateUserViewModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public int RoleId { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
