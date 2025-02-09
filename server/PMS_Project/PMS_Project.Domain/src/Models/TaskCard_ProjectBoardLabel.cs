using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class TaskCard_ProjectBoardLabel
    {
        [Required]
        public Guid ProjectBoardLabelId { get; set; }

        [Required]
        public Guid TaskCardId { get; set; }

        [ForeignKey(nameof(ProjectBoardLabelId))]
        public ProjectBoardLabel ProjectBoardLabel { get; set; } // Navigation property

        [ForeignKey(nameof(TaskCardId))]
        public TaskCard TaskCard { get; set; } // Navigation property
    }
}