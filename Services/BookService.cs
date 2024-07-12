using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        #region Fields

        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IImageFileService _imageFileManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public BookService(IBaseRepository<Book> bookRepository,
                            IImageFileService imageFileManager,
                            IMapper mapper,
                            IMemoryCache memoryCache)
        {
            _bookRepository = bookRepository;
            _imageFileManager = imageFileManager;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        #endregion

        #region Utilities

        private string GenerateCacheKey(FilterOptions filterOptions)
        {
            return $"BookList_{filterOptions.PageNumber}_{filterOptions.PageSize}_{filterOptions.SortBy}_{filterOptions.SearchQuery.ToLower()}";
        }

        #endregion

        #region Methods

        public async Task<BaseModel<BookViewModel>> BookByIdAsync(int id)
        {
            BaseModel<BookViewModel> bookById = new BaseModel<BookViewModel>();

            var book = await _bookRepository.GetByCondition(x => x.Id == id).Include(x => x.Category).FirstOrDefaultAsync();

            bookById.Value = _mapper.Map<BookViewModel>(book);

            if (bookById.Value != null)
            {
                bookById.IsValid = true;
                bookById.ValidationMessage = "User Details Fetched Successfully.";
            }
            else
            {
                bookById.IsValid = false;
                bookById.ValidationMessage = "Something Went Wrong!";
            }

            return bookById;
        }

        public async Task<BaseListModel<BookViewModel>> BookListAsync(FilterOptions filterOptions)
        {
            var cacheKey = GenerateCacheKey(filterOptions);

            var cachedResult = _memoryCache.Get<BaseListModel<BookViewModel>>(cacheKey);

            cachedResult = null;

            if (cachedResult != null)
            {
                return cachedResult;
            }

            var books = _bookRepository.GetAll();

            if (!string.IsNullOrEmpty(filterOptions.SearchQuery))
            {
                books = books.Where(_ =>
                            _.Title != null && _.Title.ToLower().Contains(filterOptions.SearchQuery.ToLower()) ||
                            _.Details != null && _.Details.ToLower().Contains(filterOptions.SearchQuery.ToLower())
                        );
            }

            if (filterOptions.SortBy != 0)
            {
                if (filterOptions.SortBy == 1)
                {
                    books = books.OrderBy(_ => _.Title);
                }
                else if (filterOptions.SortBy == 2)
                {
                    books = books.OrderByDescending(_ => _.Title);
                }
            }

            books = books.Include(_ => _.Category);

            BaseListModel<BookViewModel> response = new BaseListModel<BookViewModel>();

            response.List = await PaginatedList<BookViewModel>.CreateAsync(books, filterOptions.PageNumber, filterOptions.PageSize, _mapper);

            if (response.List != null)
            {
                response.IsValid = true;
                response.ValidationMessage = "BookList Fetched Successfully.";
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessage = "Something Went Wrong.";
            }

            var cacheExpirationOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, response, cacheExpirationOptions);

            return response;
        }

        public async Task<BaseResponseModel> CreateAsync(CreateBookViewModel createBookViewModel)
        {
            if (createBookViewModel != null)
            {
                if (createBookViewModel.CategoryId == 0)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Please Select a Category for the Book."
                    };
                }
                else if (_bookRepository.GetByCondition(x => x.ISBN == createBookViewModel.ISBN).FirstOrDefault() != null)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Book with the same ISBN already exist."
                    };
                }

                else if (createBookViewModel.ImageFile == null)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Book Cover is missing."
                    };
                }

                Book newBook = new Book
                {
                    Title = createBookViewModel.Title,
                    Author = createBookViewModel.Author,
                    ISBN = createBookViewModel.ISBN,
                    Image = _imageFileManager.SaveImageFile(createBookViewModel.ImageFile),
                    Details = createBookViewModel.Details,
                    CategoryId = createBookViewModel.CategoryId
                };

                _bookRepository.Add(newBook);

                bool isSuccessful = await _bookRepository.SaveChangesAsync();

                if (isSuccessful)
                {
                    return new BaseResponseModel
                    {
                        IsValid = true,
                        ValidationMessage = "User created succesfully!"
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
            Book? book = await _bookRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            if (book == null)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "User Doesn't Exist"
                };
            }

            _bookRepository.Delete(book);

            bool isSuccessful = await _bookRepository.SaveChangesAsync();

            if (isSuccessful)
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

        public async Task<BaseResponseModel> UpdateAsync(UpdateBookViewModel updateBookViewModel)
        {
            if (updateBookViewModel != null)
            {
                if (updateBookViewModel.CategoryId == 0)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Please Select a Category for the User."
                    };
                }

                var existingISBNBook = await _bookRepository.GetByCondition(x => x.ISBN == updateBookViewModel.ISBN && x.Id != updateBookViewModel.Id).FirstOrDefaultAsync();

                if (existingISBNBook != null)
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Book with the same ISBN already exist."
                    };
                }

                Book? book = await _bookRepository.GetByCondition(x => x.Id == updateBookViewModel.Id).FirstOrDefaultAsync();

                if (book != null)
                {
                    book.Title = updateBookViewModel.Title;
                    book.Author = updateBookViewModel.Author;
                    book.ISBN = updateBookViewModel.ISBN;
                    if (updateBookViewModel.ImageFile != null)
                    {

                        book.Image = _imageFileManager.SaveImageFile(updateBookViewModel.ImageFile);
                    }
                    book.Details = updateBookViewModel.Details;
                    book.CategoryId = updateBookViewModel.CategoryId;

                    _bookRepository.Update(book);

                    bool isSuccessful = await _bookRepository.SaveChangesAsync();

                    if (isSuccessful)
                    {
                        return new BaseResponseModel
                        {
                            IsValid = true,
                            ValidationMessage = "Book Updated Successfully!"
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

        #endregion
    }
}
