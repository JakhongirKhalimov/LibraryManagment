using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.ViewModels.UserViewModels
{
    public class UserViewModel : BaseEntity
    {
        public string? UserName { get; set; }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
