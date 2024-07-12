using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.UserViewModels;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;

namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public UserService(IBaseRepository<User> userRepository,
                            IBaseRepository<Role> roleRepository,
                            IHttpContextAccessor contextAccessor,
                            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<BaseResponseModel> CreateAsync(CreateUserViewModel createUserViewModel)
        {
            if (createUserViewModel != null)
            {
                if (createUserViewModel.Password != createUserViewModel.ConfirmPassword)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Both Passwords Didn't Match!"
                    };
                }
                else if (createUserViewModel.RoleId == 0)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Please Select a Role for the User."
                    };
                }
                else
                {
                    User? user = await _userRepository.GetByCondition(_ => _.UserName != null &&
                                                 _.UserName.Equals(createUserViewModel.UserName))
                                                .FirstOrDefaultAsync();
                    if(user != null)
                    {
                        return new BaseResponseModel
                        {
                            IsValid = false,
                            ValidationMessage = "Username is not available, Please use another!"
                        };
                    }
                }

                User newUser = _mapper.Map<User>(createUserViewModel);

                _userRepository.Add(newUser);
                if (await _userRepository.SaveChangesAsync())
                {
                    return new BaseResponseModel
                    {
                        IsValid = true,
                        ValidationMessage = "User Created Successfully!"
                    };
                }
            }

            return new BaseResponseModel
            {
                IsValid = false,
                ValidationMessage = "Something went wrong, please try again later"
            };
        }

        public async Task<BaseResponseModel> DeleteAsync(int id)
        {
            var adminCount = await _userRepository.GetByCondition(x => x.RoleId == 1).CountAsync();

            User? user = await _userRepository.GetByCondition(_ => _.Id.Equals(id)).FirstOrDefaultAsync();

            if (user == null)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "User Doesn't Exist"
                };
            }

            if (user.RoleId == 1 && adminCount <= 1)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "You can't delete the only admin user!"
                };
            }

            var currUserName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();


            if (currUserName != null && user.UserName == currUserName)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "You can't delete yourself!"
                };
            }

            _userRepository.Delete(user);

            if (await _userRepository.SaveChangesAsync())
            {
                return new BaseResponseModel
                {
                    IsValid = true,
                    ValidationMessage = "User Deleted Successfully!"
                };
            }

            return new BaseResponseModel
            {
                IsValid = false,
                ValidationMessage = "Something went wrong, please try again later"
            };
        }

        public async Task<List<Role>> RoleListAsync()
        {
            return await _roleRepository.GetAll().ToListAsync();
        }

        public async Task<BaseResponseModel> UpdateAsync(UpdateUserViewModel updateUserViewModel)
        {
            if (updateUserViewModel != null)
            {
                if (updateUserViewModel.RoleId == 0)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Please Select a Role for the User."
                    };
                }

                User? user = await _userRepository.GetByCondition(x => x.Id.Equals(updateUserViewModel.Id)).FirstOrDefaultAsync();

                if (user != null)
                {
                    user.RoleId = updateUserViewModel.RoleId;

                    _userRepository.Update(user);

                    if (await _userRepository.SaveChangesAsync())
                    {
                        return new BaseResponseModel
                        {
                            IsValid = true,
                            ValidationMessage = "User Created Successfully!"
                        };
                    }

                }
            }

            return new BaseResponseModel
            {
                IsValid = false,
                ValidationMessage = "Something went wrong, please try again later"
            };
        }

        public async Task<BaseModel<UserViewModel>> UserByIdAsync(int id)
        {
            var userByIdResponse = new BaseModel<UserViewModel>();

            var user = await _userRepository.GetByCondition(x => x.Id == id).Include(x => x.Role).FirstOrDefaultAsync();

            userByIdResponse.Value = _mapper.Map<UserViewModel>(user);

            if (userByIdResponse.Value != null)
            {
                userByIdResponse.IsValid = true;
                userByIdResponse.ValidationMessage = "User Details Fetched Successfully.";
            }
            else
            {
                userByIdResponse.IsValid = false;
                userByIdResponse.ValidationMessage = "Something Went Wrong!";
            }

            return userByIdResponse;
        }

        public async Task<BaseListModel<UserViewModel>> UserListAsync(FilterOptions filterOptions)
        {
            var users = _userRepository.GetAll();

            if (!string.IsNullOrEmpty(filterOptions.SearchQuery))
            {
                users = users.Where(_ => _.UserName != null && _.UserName.ToLower().Contains(filterOptions.SearchQuery.ToLower()));
            }

            if (filterOptions.SortBy != 0)
            {
                if (filterOptions.SortBy == 1)
                {
                    users = users.OrderBy(_ => _.UserName);
                }
                else if (filterOptions.SortBy == 2)
                {
                    users = users.OrderByDescending(_ => _.UserName);
                }
            }

            users = users.Include(_ => _.Role);

            var response = new BaseListModel<UserViewModel>();

            response.List = await PaginatedList<UserViewModel>.CreateAsync(users, filterOptions.PageNumber, filterOptions.PageSize, _mapper);

            if (response.List != null)
            {
                response.IsValid = true;
                response.ValidationMessage = "UserList Fetched Successfully.";
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessage = "Something Went Wrong.";
            }

            return response;
        }
        #endregion
    }
}
