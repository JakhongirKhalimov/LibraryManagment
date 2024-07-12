using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels.FavouriteBookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class FavouriteBookController : Controller
    {
        #region Fields
        private readonly IFavouriteBookService _favouriteBookService;
        #endregion

        #region Constructor
        public FavouriteBookController(IFavouriteBookService favouriteBookService)
        {
            _favouriteBookService = favouriteBookService;
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<IActionResult> ToggleFavourite(int id)
        {
            var response = await _favouriteBookService.ToggleFavouriteAsync(id);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFavouriteBookViewModel createFavouriteBookViewModel)
        {
            var response = await _favouriteBookService.CreateAsync(createFavouriteBookViewModel);
            return Json(response);
        }
        #endregion
    }
}
