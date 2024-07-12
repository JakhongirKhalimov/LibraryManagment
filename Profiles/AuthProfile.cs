using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels.AuthViewModels;

namespace LibraryManagementSystem.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, LoginModel>()
                .ForMember(dest =>
                dest.IsValid,
                opt => opt.MapFrom(_ => true))
                .ForMember(dest =>
                dest.ValidationMessage,
                opt => opt.MapFrom(_ => "Login Successful"));
        }
    }
}
