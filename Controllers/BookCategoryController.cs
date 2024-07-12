using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookCategoryViweModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class BookCategoryController : Controller
    {
        #region Fields
        private IBookCategoryService _bookCategoryService;
        #endregion
        #region Constructor
        public BookCategoryController(IBookCategoryService bookCategoryService)
        {
            _bookCategoryService = bookCategoryService;
        }
        #endregion
        #region Methods
        [HttpGet]
        public async Task<IActionResult> BookCategoryList([FromQuery] FilterOptions filterOptions)
        {
            var bookCategoryList = await _bookCategoryService.BookCategoryListAsync(filterOptions);

            ViewData["sortBy"] = filterOptions.SortBy;

            ViewData["searchQuery"] = filterOptions.SearchQuery;

            return View(bookCategoryList.List);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookCategoryViewModel createBookCategoryViewModel)
        {
            var response = await _bookCategoryService.CreateAsync(createBookCategoryViewModel);

            if (response != null && response.IsValid)
            {
                return RedirectToAction("BookCategoryList", "BookCategory");
            }

            if (response != null) ViewData["ValidationMessage"] = response.ValidationMessage;

            return View(createBookCategoryViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var bookCategoryById = await _bookCategoryService.BookCategoryByIdAsync(id);

            if (bookCategoryById != null && bookCategoryById.IsValid && bookCategoryById.Value != null)
            {
                UpdateBookCategoryViewModel updateBookCategoryViewModel = new UpdateBookCategoryViewModel()
                {
                    Id = bookCategoryById.Value.Id,
                    Name = bookCategoryById.Value.Name
                };

                return View(updateBookCategoryViewModel);
            }

            return RedirectToAction("BookCategoryList", "BookCategory");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBookCategoryViewModel updateBookCategoryView)
        {
            var response = await _bookCategoryService.UpdateAsync(updateBookCategoryView);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            if (response.IsValid)
            {
                return RedirectToAction("BookCategoryList", "BookCategory");
            }

            return View(updateBookCategoryView);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _bookCategoryService.DeleteAsync(id);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            return RedirectToAction("BookCategoryList", "BookCategory");
        }
        #endregion
    }
}
