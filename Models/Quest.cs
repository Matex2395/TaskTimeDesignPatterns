using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTimePredicter.Models
{
    public class Quest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestId { get; set; }
        [Required]
        public required string QuestName { get; set; }
        [Required]
        public double EstimatedTime { get; set; }
        public double? ActualTime { get; set; }
        [Required]
        public required string QuestState { get; set; }
        public DateOnly CreationDate { get; set; }
        
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int? SubcategoryId { get; set; }
        public virtual Subcategory? Subcategory { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project? Project { get; set; }

    }
}
