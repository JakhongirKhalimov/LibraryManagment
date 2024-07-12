using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookViewModels;

namespace LibraryManagementSystem.Services
{
    public interface IHomeService
    {
        Task<BaseListModel<BookViewModel>> BookListAsync(FilterOptions filterOptions);
        Task<BaseModel<BookViewModel>> BookByIdAsync(int id);
    }
}
