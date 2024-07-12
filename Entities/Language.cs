using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(Language))]
    public class Language : BaseEntity
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
    }
}
