using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels.FavouriteBookViewModels;

namespace LibraryManagementSystem.Profiles
{
    public class FavouriteBookProfile : Profile
    {
        public FavouriteBookProfile()
        {
            CreateMap<FavouriteBook, FavouriteBookViewModel>();
        }
    }
}
