using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] FilterOptions filterOptions)
        {
            var response = await _notificationService.GetNotificationsAsync(filterOptions);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleRead(int id)
        {
            var response = await _notificationService.ToggleReadAsync(id);

            return Json(response);
        }
    }
}
