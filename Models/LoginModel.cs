using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Models
{
    public class LoginModel : BaseResponseModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public Role? Role { get; set; }
    }
}
