using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(FavouriteBook))]
    public class FavouriteBook : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book? Book { get; set;}
        public bool IsFavourite { get; set; }
    }
}
