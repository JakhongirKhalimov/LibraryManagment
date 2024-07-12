using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Events;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels.AuthViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        #region Constructor
        public AuthService(IBaseRepository<User> userRepository, IMapper mapper, IEventPublisher eventPublisher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }
        #endregion
        #region Methods
        public async Task<LoginModel> LoginAsync(LoginViewModel loginViewModel)
        {
            User? user = await _userRepository.GetByCondition(x => x.UserName == loginViewModel.UserName).Include(x => x.Role).FirstOrDefaultAsync();

            if (user != null && BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.Password))
            {
                var loginSuccessEvent = new LoginSuccessEvent(user);
                await _eventPublisher.PublishAsync(loginSuccessEvent);

                return _mapper.Map<LoginModel>(user);
            }

            return new LoginModel
            {
                IsValid = false,
                ValidationMessage = "Invalid Credentials."
            };

        }

        public async Task<BaseResponseModel> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.UserName != null && registerViewModel.UserName.Any(x => char.IsWhiteSpace(x)))
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "Username should be full!"
                };
            }
            else if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "Both Passwords Didn't Match!"
                };
            }

            if (_userRepository.GetByCondition(x => x.UserName == registerViewModel.UserName).FirstOrDefault() != null)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "Username is not available, Please use another!"
                };
            }
            // add new user 
            User newUser = new User
            {
                UserName = registerViewModel.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(registerViewModel.Password),
                RoleId = 3,
            };

            _userRepository.Add(newUser);

            bool isSuccessful = await _userRepository.SaveChangesAsync();

            if (isSuccessful)
            {
                return new BaseResponseModel
                {
                    IsValid = true,
                    ValidationMessage = "Registration Completed successfully!"
                };
            }

            return new BaseResponseModel
            {
                IsValid = false,
                ValidationMessage = "Something went wrong, please try again later"
            };
        }
        #endregion
    }
}
