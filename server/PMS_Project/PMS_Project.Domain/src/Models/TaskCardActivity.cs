using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class TaskCardActivity : BaseClass
    {
        [Required]
        public Guid TaskCardId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Activity { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // Navigation property

        [ForeignKey(nameof(TaskCardId))]
        public TaskCard TaskCard { get; set; } // Navigation property
    }

}