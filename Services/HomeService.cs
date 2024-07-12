using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class HomeService : IHomeService
    {
        #region Fields
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public HomeService(IBaseRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        public async Task<BaseModel<BookViewModel>> BookByIdAsync(int id)
        {
            var bookById = new BaseModel<BookViewModel>();

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

            var response = new BaseListModel<BookViewModel>();

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

            return response;
        }
        #endregion
    }
}
