using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class TaskCard : BaseClass
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public Guid ListId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Guid PriorityId { get; set; }

        public string? DueDate { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        public int Position { get; set; }

        

        [ForeignKey(nameof(ListId))]
        public AppList List { get; set; } //'List' is a reserved keyword in C# so we use 'AppList' instead

        [ForeignKey(nameof(StatusId))]
        public Status? Status { get; set; } // Navigation property

        [ForeignKey(nameof(PriorityId))]
        public Priority? Priority { get; set; } // Navigation property

        public ICollection<TaskCard_ProjectBoardLabel>? TaskCard_ProjectBoardLabels { get; set; } // Navigation property
        public ICollection<TaskCard_User>? TaskCard_Users { get; set; } // Navigation property for many-to-many relationship with User
        public ICollection<TaskCardActivity>? Activities { get; set; } // Navigation property for one-to-many relationship
        public ICollection<TaskCardChecklist>? Checklists { get; set; } // Navigation property for one-to-many relationship
    }
}