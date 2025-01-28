using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTimePredicter.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        public required string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
        public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
