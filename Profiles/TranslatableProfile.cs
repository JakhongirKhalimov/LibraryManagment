using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels.TranslatableViewModels;

namespace LibraryManagementSystem.Profiles
{
    public class TranslatableProfile : Profile
    {
        public TranslatableProfile()
        {
            CreateMap<Translatable, TranslatableViewModel>();
        }
    }
}
