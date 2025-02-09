using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class ProjectBoard_User
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProjectBoardId { get; set; }

        [Required]
        // public Guid RoleId { get; set; }

        

        [ForeignKey(nameof(UserId))] 
        public User User { get; set; } // Navigation property

        [ForeignKey(nameof(ProjectBoardId))]
        public ProjectBoard ProjectBoard { get; set; } // Navigation property

        // [ForeignKey(nameof(RoleId))]
        // public Role Role { get; set; } // Navigation property
    }
}