using AutoMapper;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        #region Fileds
        private IUserService _userService;
        #endregion

        #region Constructor
        public UserController(IMapper mapper, IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> UserList([FromQuery] FilterOptions filterOptions)
        {
            var userList = await _userService.UserListAsync(filterOptions);

            ViewData["sortBy"] = filterOptions.SortBy;

            ViewData["searchQuery"] = filterOptions.SearchQuery;

            return View(userList.List);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id = 0)
        {
            if (id == 0) return RedirectToAction("UserList", "User");

            var userResponse = await _userService.UserByIdAsync(id);

            if(userResponse != null && userResponse.IsValid)
            {
                return View(userResponse.Value);
            }

            return RedirectToAction("UserList", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateUserViewModel createUserViewModel = new CreateUserViewModel()
            {
                Roles = await _userService.RoleListAsync()
            };

            return View(createUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            var response = await _userService.CreateAsync(createUserViewModel);

            if (response != null && response.IsValid)
            {
                return RedirectToAction("UserList", "User");
            }

            if (response != null) ViewData["ValidationMessage"] = response.ValidationMessage;

            if(createUserViewModel == null) createUserViewModel = new CreateUserViewModel();

            if(createUserViewModel.Roles == null) createUserViewModel.Roles = await _userService.RoleListAsync();

            if(response != null) ViewData["ValidationMessage"] = response.ValidationMessage;

            return View(createUserViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userService.UserByIdAsync(id);

            if (user != null && user.IsValid && user.Value != null)
            {
                UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel()
                {
                    Id = user.Value.Id,
                    UserName = user.Value.UserName,
                    RoleId = user.Value.RoleId,
                    Roles = await _userService.RoleListAsync()
                };

                return View(updateUserViewModel);
            }

            return RedirectToAction("UserList", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel updateUserViewModel)
        {
            var response = await _userService.UpdateAsync(updateUserViewModel);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            if (response.IsValid)
            {
                return RedirectToAction("UserList", "User");
            }

            if(updateUserViewModel.Roles == null)
            {
                updateUserViewModel.Roles = await _userService.RoleListAsync();
            }
            return View(updateUserViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.DeleteAsync(id);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            return RedirectToAction("UserList", "User");
        }
        #endregion
    }
}
