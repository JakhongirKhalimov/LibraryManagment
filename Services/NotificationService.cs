using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Events;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.NotificationViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementSystem.Services
{
    public class NotificationService : INotificationService,
                                       IConsumer<FavouriteBookToggledEvent>,
                                       IConsumer<LoginSuccessEvent>
    {
        #region Fields
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public NotificationService(IBaseRepository<Notification> notificationRepository,
                                    IBaseRepository<User> userRepository,
                                    IMapper mapper,
                                    IHttpContextAccessor contextAccessor)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        #endregion

        #region Methods
        public async Task<BaseListModel<NotificationViewModel>> GetNotificationsAsync(FilterOptions filterOptions)
        {
            filterOptions = new FilterOptions
            {
                SortBy = 2,
                PageSize = 10
            };

            var notifications = _notificationRepository.GetAll();

            var currUserName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();

            var currUser = await _userRepository.GetByCondition(_ => _.UserName == currUserName).FirstOrDefaultAsync();

            if(currUser != null)
            {
                notifications = notifications.Where(_ => _.UserId == currUser.Id);
            }

            if (filterOptions.SortBy != 0)
            {
                if (filterOptions.SortBy == 1)
                {
                    notifications = notifications.OrderBy(_ => _.Id);
                }
                else if (filterOptions.SortBy == 2)
                {
                    notifications = notifications.OrderByDescending(_ => _.Id);
                }
            }

            BaseListModel<NotificationViewModel> response = new BaseListModel<NotificationViewModel>();

            response.List = await PaginatedList<NotificationViewModel>.CreateAsync(notifications, filterOptions.PageNumber, filterOptions.PageSize, _mapper);

            if (response.List != null)
            {
                response.IsValid = true;
                response.ValidationMessage = "Notifications Fetched Successfully.";
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessage = "Something Went Wrong.";
            }

            return response;
        }

        public async Task<BaseResponseModel> ToggleReadAsync(int id)
        {
            Notification? notification = await _notificationRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            if (notification != null)
            {
                notification.IsMarkedRead = !notification.IsMarkedRead;

                _notificationRepository.Update(notification);

                bool isSuccessful = await _notificationRepository.SaveChangesAsync();

                if (isSuccessful)
                {
                    return new BaseResponseModel
                    {
                        IsValid = true,
                        ValidationMessage = "Notification Status Updated Successfully!"
                    };
                }
            }

            return new BaseResponseModel
            {
                IsValid = false,
                ValidationMessage = "Something went wrong!"
            };
        }
        #endregion

        #region EventHandlers
        public async Task HandleEventAsync(FavouriteBookToggledEvent @event)
        {
            if (@event != null)
            {
                Notification newNotification = new Notification
                {
                    UserId = @event.FavouriteBook.UserId,
                    IsMarkedRead = false
                };
                if (@event.FavouriteBook.IsFavourite)
                {
                    newNotification.Text = "Wow, " + @event.Book.Title + " is your favourite!";
                }
                else
                {
                    newNotification.Text = "You removed " + @event.Book.Title + " from your favourite!";
                }

                _notificationRepository.Add(newNotification);

                await _notificationRepository.SaveChangesAsync();
            }
        }

        public async Task HandleEventAsync(LoginSuccessEvent @event)
        {
            if (@event != null)
            {
                Notification newNotification = new Notification
                {
                    UserId = @event.User.Id,
                    IsMarkedRead = false
                };

                newNotification.Text = "Hello " + @event.User.UserName + ", welcome back!";

                _notificationRepository.Add(newNotification);

                await _notificationRepository.SaveChangesAsync();
            }
        }
        #endregion
    }
}
