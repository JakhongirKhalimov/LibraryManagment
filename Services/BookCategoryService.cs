using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookCategoryViweModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        #region Fields
        private readonly IBaseRepository<BookCategory> _bookCategoryRepository;
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public BookCategoryService(IBaseRepository<BookCategory> bookCategoryRepository,
                                    IBaseRepository<Book> bookRepository,
                                    IMapper mapper)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        public async Task<BaseModel<BookCategoryViewModel>> BookCategoryByIdAsync(int id)
        {
            var bookCategoryById = new BaseModel<BookCategoryViewModel>();

            var bookCategory = await _bookCategoryRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            bookCategoryById.Value = _mapper.Map<BookCategoryViewModel>(bookCategory);

            if (bookCategoryById.Value != null)
            {
                bookCategoryById.IsValid = true;
                bookCategoryById.ValidationMessage = "User Details Fetched Successfully.";
            }
            else
            {
                bookCategoryById.IsValid = false;
                bookCategoryById.ValidationMessage = "Something Went Wrong!";
            }

            return bookCategoryById;
        }

        public async Task<BaseListModel<BookCategoryViewModel>> BookCategoryListAsync(FilterOptions filterOptions)
        {
            if(filterOptions == null)
            {
                filterOptions = new FilterOptions();
            }

            var bookcategories = _bookCategoryRepository.GetAll();

            if (filterOptions != null && !string.IsNullOrEmpty(filterOptions.SearchQuery))
            {
                bookcategories = bookcategories.Where(_ =>
                            _.Name != null && _.Name.ToLower().Contains(filterOptions.SearchQuery.ToLower()));
            }

            if (filterOptions != null && filterOptions.SortBy != 0)
            {
                if (filterOptions.SortBy == 1)
                {
                    bookcategories = bookcategories.OrderBy(_ => _.Name);
                }
                else if (filterOptions.SortBy == 2)
                {
                    bookcategories = bookcategories.OrderByDescending(_ => _.Name);
                }
            }

            BaseListModel<BookCategoryViewModel> response = new BaseListModel<BookCategoryViewModel>();

            response.List = await PaginatedList<BookCategoryViewModel>.CreateAsync(bookcategories, filterOptions.PageNumber, filterOptions.PageSize, _mapper);

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

        public async Task<BaseResponseModel> CreateAsync(CreateBookCategoryViewModel createBookCategoryViewModel)
        {
            if (createBookCategoryViewModel != null)
            {
                BookCategory newBookCategory = new BookCategory
                {
                    Name = createBookCategoryViewModel.Name
                };

                _bookCategoryRepository.Add(newBookCategory);

                bool isSuccessful = await _bookCategoryRepository.SaveChangesAsync();

                if (isSuccessful)
                {
                    return new BaseResponseModel
                    {
                        IsValid = true,
                        ValidationMessage = "Book Category Created Successfully!"
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
            if (id == 1)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "You are not Allowed to Delete Default Category!"
                };
            }

            BookCategory? bookCategory = await _bookCategoryRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            if (bookCategory == null)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "Book Category Doesn't Exist"
                };
            }

            List<Book> booksInThecategory = await _bookRepository.GetByCondition(x => x.CategoryId == id).ToListAsync();

            foreach (Book book in booksInThecategory)
            {
                book.CategoryId = 1;
                _bookRepository.Update(book);
            }

            _bookCategoryRepository.Delete(bookCategory);

            bool isSuccessful = await _bookCategoryRepository.SaveChangesAsync();

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

        public async Task<BaseResponseModel> UpdateAsync(UpdateBookCategoryViewModel updateBookCategoryViewModel)
        {
            if (updateBookCategoryViewModel != null)
            {
                BookCategory? bookCategory = await _bookCategoryRepository.GetByCondition(x => x.Id == updateBookCategoryViewModel.Id).FirstOrDefaultAsync();

                if (bookCategory != null)
                {
                    bookCategory.Name = updateBookCategoryViewModel.Name;

                    _bookCategoryRepository.Update(bookCategory);

                    bool isSuccessful = await _bookCategoryRepository.SaveChangesAsync();

                    if (isSuccessful)
                    {
                        return new BaseResponseModel
                        {
                            IsValid = true,
                            ValidationMessage = "Book Category Updated Successfully!"
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
