using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.LanguageViewModels;

namespace LibraryManagementSystem.Services
{
    public interface ILanguageService
    {
        Task<BaseListModel<LanguageViewModel>> LanguageListAsync(FilterOptions filterOptions);
        Task<BaseModel<LanguageViewModel>> LanguageByIdAsync(int id);
        Task<BaseResponseModel> CreateAsync(CreateLanguageViewModel createLanguageViewModel);
        Task<BaseResponseModel> UpdateAsync(UpdateLanguageViewModel updateLanguageViewModel);
    }
}
