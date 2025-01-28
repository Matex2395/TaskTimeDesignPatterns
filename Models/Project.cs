using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTimePredicter.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        [Required]
        public required string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
    }
}
