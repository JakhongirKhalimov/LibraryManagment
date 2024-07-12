using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels.NotificationViewModels;

namespace LibraryManagementSystem.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationViewModel>();
        }
    }
}
