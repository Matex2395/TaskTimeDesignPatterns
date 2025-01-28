using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTimePredicter.Models
{
    public class Subcategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubcategoryId { get; set; }
        [Required]
        public required string SubcategoryName { get; set; }
        public string? SubcategoryDescription { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
    }
}
