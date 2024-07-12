using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.ViewModels.TranslatableViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class TranslatableService : ITranslatableService
    {
        #region Fields
        private readonly IBaseRepository<Translatable> _translatableRepositopry;
        private readonly IReflectionRepository _reflectionRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public TranslatableService(IBaseRepository<Translatable> translatableRepositopry,
                                   IReflectionRepository reflectionRepository,
                                   IMapper mapper)
        {
            _translatableRepositopry = translatableRepositopry;
            _reflectionRepository = reflectionRepository;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<BaseResponseModel> CreateAsync(CreateTranslatableViewModel createTranslatableViewModel)
        {
            if (createTranslatableViewModel != null)
            {
                if (await _translatableRepositopry.GetByCondition(_ => _.Entity == createTranslatableViewModel.Entity
                            && _.Attribute == createTranslatableViewModel.Attribute).AnyAsync())
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                        ValidationMessage = "Can't create duplicate data!"
                    };
                }

                Translatable newTranslatable = new Translatable
                {
                    Entity = createTranslatableViewModel.Entity,
                    Attribute = createTranslatableViewModel.Attribute
                };

                _translatableRepositopry.Add(newTranslatable);

                if (await _translatableRepositopry.SaveChangesAsync())
                {
                    return new BaseResponseModel
                    {
                        IsValid = true,
                        ValidationMessage = "Data Added Successfully!"
                    };
                }
            }

            return new BaseResponseModel
            {
                IsValid = false,
                ValidationMessage = "Something Went Wrong!"
            };
        }

        public async Task<BaseResponseModel> DeleteAsync(int id)
        {
            Translatable? translatable = await _translatableRepositopry.GetByCondition(_ => _.Id == id).FirstOrDefaultAsync();

            if (translatable == null)
            {
                return new BaseResponseModel
                {
                    IsValid = false,
                    ValidationMessage = "Data doesn't exists"
                };
            }

            _translatableRepositopry.Delete(translatable);

            if (await _translatableRepositopry.SaveChangesAsync())
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

        public List<string> GetAttributes(string Entity)
        {
            List<string> attributes = new List<string>();

            var entityType = _reflectionRepository.GetEntities()
                                    .Where(prop => prop.PropertyType.IsGenericType
                                        && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)
                                        && prop.Name == Entity)
                                    .Select(prop => prop.PropertyType.GetGenericArguments().First())
                                    .FirstOrDefault();

            var attrInDb = _translatableRepositopry.GetByCondition(_ => _.Entity == Entity)
                                    .Select(_ => _.Attribute)
                                    .ToList();

            if (entityType != null)
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.PropertyType.Name == "String" && !attrInDb.Contains(property.Name))
                    {
                        attributes.Add(property.Name);
                    }
                }
            }

            return attributes;
        }

        public List<string> GetEntities()
        {
            return _reflectionRepository.GetEntities()
                            .Where(_ => _.PropertyType.IsGenericType
                            && _.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                            .Select(_ => _.Name).ToList();
        }

        public async Task<BaseListModel<TranslatableViewModel>> GetTranslatableListAsync(FilterOptions filterOptions)
        {
            var translatables = _translatableRepositopry.GetAll();

            if (!string.IsNullOrEmpty(filterOptions.SearchQuery))
            {
                translatables = translatables.Where(_ =>
                            _.Entity != null && _.Entity.ToLower().Contains(filterOptions.SearchQuery.ToLower())
                            || _.Attribute != null && _.Attribute.ToLower().Contains(filterOptions.SearchQuery.ToLower()));
            }

            if (filterOptions.SortBy != 0)
            {
                if (filterOptions.SortBy == 1)
                {
                    translatables = translatables.OrderBy(_ => _.Entity).ThenBy(_ => _.Attribute);
                }
                else if (filterOptions.SortBy == 2)
                {
                    translatables = translatables.OrderByDescending(_ => _.Entity).ThenByDescending(_ => _.Attribute);
                }
            }

            var response = new BaseListModel<TranslatableViewModel>();

            response.List = await PaginatedList<TranslatableViewModel>.CreateAsync(translatables, filterOptions.PageNumber, filterOptions.PageSize, _mapper);

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
        #endregion
    }
}
