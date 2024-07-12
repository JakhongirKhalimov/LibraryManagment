using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(Notification))]
    public class Notification : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        public string? Text { get; set; }
        public bool IsMarkedRead { get; set; }
    }
}
