using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class TaskCardChecklist : BaseClass
    {
        [Required]
        public Guid TaskCardId { get; set; }

        [Required]
        [MaxLength(60)]
        public string Title { get; set; }

        [ForeignKey(nameof(TaskCardId))]
        public TaskCard TaskCard { get; set; } // Navigation property
        
        public ICollection<TaskCardChecklistItem> Items { get; set; } // Navigation property
    }
}