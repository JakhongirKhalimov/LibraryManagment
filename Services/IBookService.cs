using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookViewModels;

namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<BaseListModel<BookViewModel>> BookListAsync(FilterOptions filterOptions);
        Task<BaseModel<BookViewModel>> BookByIdAsync(int id);
        Task<BaseResponseModel> CreateAsync(CreateBookViewModel createBookViewModel);
        Task<BaseResponseModel> DeleteAsync(int id);
        Task<BaseResponseModel> UpdateAsync(UpdateBookViewModel updateBookViewModel);
    }
}
