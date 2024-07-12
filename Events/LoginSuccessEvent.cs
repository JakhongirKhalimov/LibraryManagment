 using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Events
{
    public class LoginSuccessEvent
    {
        public LoginSuccessEvent(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
