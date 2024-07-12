using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(Translatable))]
    public class Translatable : BaseEntity
    {
        public string? Entity { get; set; }
        public string? Attribute { get; set; }
    }
}
