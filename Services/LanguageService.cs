using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.LanguageViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class LanguageService : ILanguageService
    {
        #region Fields
        private readonly IBaseRepository<Language> _languageRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public LanguageService(IBaseRepository<Language> languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<BaseResponseModel> CreateAsync(CreateLanguageViewModel createLanguageViewModel)
        {
            if (createLanguageViewModel != null)
            {
                Language newLanguage = new Language
                {
                    Name = createLanguageViewModel.Name,
                    ShortName = createLanguageViewModel.ShortName

                };

                _languageRepository.Add(newLanguage);

                bool isSuccess = await _languageRepository.SaveChangesAsync();

                if (isSuccess)
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

        public async Task<BaseModel<LanguageViewModel>> LanguageByIdAsync(int id)
        {
            var languageById = new BaseModel<LanguageViewModel>();

            var language = await _languageRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            languageById.Value = _mapper.Map<LanguageViewModel>(language);

            if (languageById.Value != null)
            {
                languageById.IsValid = true;
                languageById.ValidationMessage = "Language Details Fetched Successfully.";
            }
            else
            {
                languageById.IsValid = false;
                languageById.ValidationMessage = "Something Went Wrong!";
            }

            return languageById;
        }

        public async Task<BaseListModel<LanguageViewModel>> LanguageListAsync(FilterOptions filterOptions)
        {
            var languages = _languageRepository.GetAll();

            if (!string.IsNullOrEmpty(filterOptions.SearchQuery))
            {
                languages = languages.Where(_ =>
                            _.Name != null && _.Name.ToLower().Contains(filterOptions.SearchQuery.ToLower()) ||
                            _.ShortName != null && _.ShortName.ToLower().Contains(filterOptions.SearchQuery.ToLower()));
            }

            if (filterOptions.SortBy != 0)
            {
                if (filterOptions.SortBy == 1)
                {
                    languages = languages.OrderBy(_ => _.Name).ThenBy(_ => _.ShortName);
                }
                else if (filterOptions.SortBy == 2)
                {
                    languages = languages.OrderByDescending(_ => _.Name).ThenByDescending(_ => _.ShortName);
                }
            }

            var response = new BaseListModel<LanguageViewModel>();

            response.List = await PaginatedList<LanguageViewModel>.CreateAsync(languages, filterOptions.PageNumber, filterOptions.PageSize, _mapper);

            if (response.List != null)
            {
                response.IsValid = true;
                response.ValidationMessage = "Languages Fetched Successfully.";
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessage = "Something Went Wrong.";
            }

            return response;
        }

        public async Task<BaseResponseModel> UpdateAsync(UpdateLanguageViewModel updateLanguageViewModel)
        {
            if (updateLanguageViewModel != null)
            {
                Language? language = await _languageRepository.GetByCondition(x => x.Id == updateLanguageViewModel.Id).FirstOrDefaultAsync();

                if (language != null)
                {
                    language.Name = updateLanguageViewModel.Name;
                    language.ShortName = updateLanguageViewModel.ShortName;

                    _languageRepository.Update(language);

                    bool isSuccessful = await _languageRepository.SaveChangesAsync();

                    if (isSuccessful)
                    {
                        return new BaseResponseModel
                        {
                            IsValid = true,
                            ValidationMessage = "Language Updated Successfully!"
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
