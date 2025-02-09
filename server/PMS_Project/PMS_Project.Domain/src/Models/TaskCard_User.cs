using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class TaskCard_User
    {
        [Required]
        public Guid TaskCardId { get; set; }
     
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(TaskCardId))]
        public TaskCard TaskCard { get; set; } // Navigation property

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // Navigation property
    }
}