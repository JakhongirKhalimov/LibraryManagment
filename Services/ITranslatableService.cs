using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.TranslatableViewModels;

namespace LibraryManagementSystem.Services
{
    public interface ITranslatableService
    {
        Task<BaseListModel<TranslatableViewModel>> GetTranslatableListAsync(FilterOptions filterOptions);
        Task<BaseResponseModel> CreateAsync(CreateTranslatableViewModel createTranslatableViewModel);
        Task<BaseResponseModel> DeleteAsync(int id);
        List<string> GetEntities();
        List<string> GetAttributes(string Entity);
    }
}
