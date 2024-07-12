using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        #region Fields
        private readonly IAuthService _authService;
        #endregion
        #region Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion
        #region Methoods
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity != null && claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var login = await _authService.LoginAsync(loginViewModel);

            if(login != null && login.IsValid && login.UserName != null && login.Role != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, login.UserName),
                    new Claim(ClaimTypes.Role, login.Role.Name)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, 
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = loginViewModel.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            if(login != null) ViewData["ValidationMessage"] = login.ValidationMessage;
            return View();
            
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity != null && claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var response = await _authService.RegisterAsync(registerViewModel);

            if (response != null && response.IsValid)
            {
                return await Login(new LoginViewModel
                {
                    UserName = registerViewModel.UserName,
                    Password = registerViewModel.Password
                });
            }

            if(response != null)ViewData["ValidationMessage"] = response.ValidationMessage;

            return View();

        }
        #endregion
    }
}
