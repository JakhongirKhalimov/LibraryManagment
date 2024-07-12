using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(BookCategory))]
    public class BookCategory : BaseEntity
    {
        public string? Name { get; set; }
    }
}
