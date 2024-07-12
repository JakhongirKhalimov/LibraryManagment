using LibraryManagementSystem.Events;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels.FavouriteBookViewModels;

namespace LibraryManagementSystem.Services
{
    public interface IFavouriteBookService
    {
        //event EventHandler<CreateFavouriteBookEventArgs> FavouriteBookCreated;
        Task<BaseModel<FavouriteBookViewModel>> IsFavouriteAsync(int BookId);
        Task<BaseResponseModel> ToggleFavouriteAsync(int BookId);
        Task<BaseResponseModel> CreateAsync(CreateFavouriteBookViewModel createFavouriteBookViewModel);
    }
}
