using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.ViewModels.UserViewModels
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int RoleId { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
