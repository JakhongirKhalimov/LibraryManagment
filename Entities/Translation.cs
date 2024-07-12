using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    [Table(nameof(Translation))]
    public class Translation : BaseEntity
    {
        public string? Entity { get; set; }
        public string? Attribute { get; set; }
        public int RowId { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public virtual Language? Language { get; set; }
        public string? Value { get; set; }
    }
}
