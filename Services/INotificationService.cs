using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.NotificationViewModels;

namespace LibraryManagementSystem.Services
{
    public interface INotificationService
    {
        Task<BaseListModel<NotificationViewModel>> GetNotificationsAsync(FilterOptions filterOptions);
        Task<BaseResponseModel> ToggleReadAsync(int id);
    }
}
