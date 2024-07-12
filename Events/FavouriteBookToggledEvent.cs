using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Events
{
    public class FavouriteBookToggledEvent
    {
        public FavouriteBookToggledEvent(Book book, FavouriteBook favouriteBook)
        {
            Book = book;
            FavouriteBook = favouriteBook;
        }

        public Book Book { get; }
        public FavouriteBook FavouriteBook { get; }
    }
}
