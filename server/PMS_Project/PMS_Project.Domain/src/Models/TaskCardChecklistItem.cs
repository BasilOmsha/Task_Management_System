using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class TaskCardChecklistItem : BaseClass
    {
        [Required]
        public Guid TaskCardChecklistId { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        public bool IsChecked { get; set; }
        public int Position { get; set; }

        [ForeignKey(nameof(TaskCardChecklistId))]
        public TaskCardChecklist TaskCardChecklist { get; set; } // Navigation property
    }
}