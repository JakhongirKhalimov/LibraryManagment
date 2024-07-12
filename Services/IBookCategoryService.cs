using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.BookCategoryViweModels;

namespace LibraryManagementSystem.Services
{
    public interface IBookCategoryService
    {
        Task<BaseListModel<BookCategoryViewModel>> BookCategoryListAsync(FilterOptions filterOptions);
        Task<BaseModel<BookCategoryViewModel>> BookCategoryByIdAsync(int id);
        Task<BaseResponseModel> CreateAsync(CreateBookCategoryViewModel createBookCategoryViewModel);
        Task<BaseResponseModel> DeleteAsync(int id);
        Task<BaseResponseModel> UpdateAsync(UpdateBookCategoryViewModel updateBookCategryViewModel);
    }
}
