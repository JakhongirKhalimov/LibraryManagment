using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(Role))]
    public class Role : BaseEntity
    {
        public string? Name { get; set; }
    }
}
