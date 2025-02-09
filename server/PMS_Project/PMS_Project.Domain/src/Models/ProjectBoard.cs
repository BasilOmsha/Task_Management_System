using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class ProjectBoard : BaseClass
    {
        [Required]
        public Guid WorkspaceId { get; set; }
        
        [Required]
        public Guid CreatorUserId { get; set; }
        
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        

        [ForeignKey(nameof(WorkspaceId))]
        public Workspace Workspace { get; set; } // Navigation property

        [ForeignKey(nameof(CreatorUserId))]
        public User User { get; set; } // Navigation property
        
        public ICollection<ProjectBoard_User> ProjectBoard_Users { get; set; } // Navigation property for many-to-many relationship with User
        public ICollection<ProjectBoardLabel> ProjectBoardLabels { get; set; } // One-to-many with ProjectBoardLabel - navigation property
        public ICollection<AppList> AppList { get; set; } // One-to-many with AppList - navigation property

    }
}