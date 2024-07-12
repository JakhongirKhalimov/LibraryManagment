using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Fields
        private readonly IHomeService _homeService;
        private readonly IFavouriteBookService _favouriteBookService;
        #endregion
        #region Constructor
        public HomeController(IHomeService homeService, IFavouriteBookService favouriteBookService)
        {
            _homeService = homeService;
            _favouriteBookService = favouriteBookService;

        }
        #endregion
        #region Methods
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] FilterOptions filterOptions)
        {
            var bookList = await _homeService.BookListAsync(filterOptions);

            ViewData["sortBy"] = filterOptions.SortBy;

            ViewData["searchQuery"] = filterOptions.SearchQuery;

            return View(bookList.List);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return RedirectToAction("Index", "Home");

            var bookById = await _homeService.BookByIdAsync(id);
            var favouriteBook = await _favouriteBookService.IsFavouriteAsync(id);

            if (bookById != null && bookById.IsValid)
            {
                if(favouriteBook != null && favouriteBook.Value != null && favouriteBook.Value.IsFavourite)
                {
                    ViewData["Liked"] = "liked";
                }
                else
                {
                    ViewData["Liked"] = "unliked";
                }
                return View(bookById.Value);
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
