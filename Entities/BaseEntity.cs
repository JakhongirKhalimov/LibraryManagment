using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
