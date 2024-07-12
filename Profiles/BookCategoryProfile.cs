using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels.BookCategoryViweModels;

namespace LibraryManagementSystem.Profiles
{
    public class BookCategoryProfile : Profile
    {
        public BookCategoryProfile()
        {
            CreateMap<BookCategory, BookCategoryViewModel>();
        }
    }
}
