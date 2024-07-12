using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class BookController : Controller
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IBookCategoryService _bookCategoryService;

        #endregion

        #region Constructor
        public BookController(IBookService bookService, IBookCategoryService bookCategoryService)
        {
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> BookList([FromQuery] FilterOptions filterOptions)
        {
            var bookList = await _bookService.BookListAsync(filterOptions);
            ViewData["sortBy"] = filterOptions.SortBy;
            ViewData["searchQuery"] = filterOptions.SearchQuery;

            return View(bookList.List);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id = 0)
        {
            if (id == 0) return RedirectToAction("BookList", "Book");

            var bookById = await _bookService.BookByIdAsync(id);

            if (bookById != null && bookById.IsValid)
            {
                return View(bookById.Value);
            }

            return RedirectToAction("BookList", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var bookCategoryList = await _bookCategoryService.BookCategoryListAsync(new FilterOptions()
            {
                PageSize = 100
            });

            CreateBookViewModel createBookViewModel = new CreateBookViewModel()
            {
                BookCategories = bookCategoryList.List
            };

            return View(createBookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel createBookViewModel)
        {
            var response = await _bookService.CreateAsync(createBookViewModel);

            if (response != null && response.IsValid)
            {
                return RedirectToAction("BookList", "Book");
            }

            if (response != null) ViewData["ValidationMessage"] = response.ValidationMessage;

            if (createBookViewModel == null) createBookViewModel = new CreateBookViewModel();

            if (createBookViewModel.BookCategories == null)
            {
                var bookCategoryList = await _bookCategoryService.BookCategoryListAsync(new FilterOptions()
                {
                    PageSize = 100
                });

                createBookViewModel.BookCategories = bookCategoryList.List;
            }

            if (response != null)
            {
                ViewData["ValidationMessage"] = response.ValidationMessage;
            }
            return View(createBookViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var bookById = await _bookService.BookByIdAsync(id);

            if (bookById != null && bookById.IsValid && bookById.Value != null)
            {
                var bookCategoryList = await _bookCategoryService.BookCategoryListAsync(new FilterOptions()
                {
                    PageSize = 100
                });

                UpdateBookViewModel updateBookViewModel = new UpdateBookViewModel()
                {
                    Title = bookById.Value.Title,
                    Author = bookById.Value.Author,
                    ISBN = bookById.Value.ISBN,
                    ImageFile = null,
                    Details = bookById.Value.Details,
                    CategoryId = bookById.Value.CategoryId,
                    BookCategories = bookCategoryList.List,
                };

                return View(updateBookViewModel);
            }

            return RedirectToAction("BookList", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBookViewModel updateBookViewModel)
        {
            var response = await _bookService.UpdateAsync(updateBookViewModel);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            if (response.IsValid)
            {
                return RedirectToAction("BookList", "Book");
            }

            if (updateBookViewModel.BookCategories == null)
            {
                var bookCategoryList = await _bookCategoryService.BookCategoryListAsync(new FilterOptions()
                {
                    PageSize = 100
                });

                updateBookViewModel.BookCategories = bookCategoryList.List;
            }

            return View(updateBookViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _bookService.DeleteAsync(id);

            ViewData["ValidationMessage"] = response.ValidationMessage;

            return RedirectToAction("BookList", "Book");
        }
        #endregion
    }
}
