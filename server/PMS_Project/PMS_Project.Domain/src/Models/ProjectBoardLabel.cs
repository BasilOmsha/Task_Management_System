using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class ProjectBoardLabel : BaseClass
    {
        [Required]
        public Guid ProjectBoardId { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        private string _color;
        
        [Required]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Invalid color hex code.")]
        public string Color
        {
            get => _color;
            set => _color = value;
        }

        [ForeignKey(nameof(ProjectBoardId))]
        public ProjectBoard ProjectBoard { get; set; } // Navigation property
        public ICollection<TaskCard_ProjectBoardLabel> TaskCard_ProjectBoardLabels { get; set; } // Navigation property
    }
}