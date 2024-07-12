using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels.BookViewModels;
using LibraryManagementSystem.ViewModels.UserViewModels;

namespace LibraryManagementSystem.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookViewModel>();
        }
    }
}
