using LibraryManagementSystem.ViewModels.BookCategoryViweModels;

namespace LibraryManagementSystem.ViewModels.BookViewModels
{
    public class UpdateBookViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Details { get; set; }
        public int CategoryId { get; set; }
        public List<BookCategoryViewModel>? BookCategories { get; set; }
    }
}
