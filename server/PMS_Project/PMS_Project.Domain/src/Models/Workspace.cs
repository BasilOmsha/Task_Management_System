using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class Workspace : BaseClass
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public Guid CreatorUserId { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(CreatorUserId))] // Foreign key for CreatorUserId
        public User User { get; set; } // One-to-many with users in the workspace - navigation property [used to have "required" as an attribute]
        public ICollection<ProjectBoard> ProjectBoards { get; set; } // One-to-many with project boards in the workspace - navigation property
        public ICollection<Workspace_User> Workspace_Users { get; set; } // Navigation property for many-to-many relationship with User
    }
}