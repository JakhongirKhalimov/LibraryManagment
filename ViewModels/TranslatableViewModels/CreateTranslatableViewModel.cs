using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels.TranslatableViewModels
{
    public class CreateTranslatableViewModel
    {
        public string? Entity { get; set; }
        public string? Attribute { get; set; }
        public List<string>? Entities { get; set; }
        public List<string>? Attributes { get; set; }
    }
}
