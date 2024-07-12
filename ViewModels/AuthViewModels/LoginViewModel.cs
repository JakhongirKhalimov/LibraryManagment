namespace LibraryManagementSystem.ViewModels.AuthViewModels
{
    public class LoginViewModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
