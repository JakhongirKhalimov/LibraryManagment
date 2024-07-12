using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels.UserViewModels;

namespace LibraryManagementSystem.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserViewModel, User>()
                    .ForMember(dest => dest.Password,
                    opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

            CreateMap<User, UserViewModel>();
        }
    }
}
