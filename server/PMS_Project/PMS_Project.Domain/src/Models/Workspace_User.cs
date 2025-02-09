using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class Workspace_User
    {
        [Required]
        public Guid UserId { get; set; } // Foreign key property, and part of composite primary key

        [Required]
        public Guid WorkspaceId { get; set; } // Foreign key property, and part of composite primary key

        [Required]
        public Guid RoleId { get; set; } // Foreign key property.
        

        [ForeignKey(nameof(UserId))] // Foreign key attribute applied on the navigation property and it will create in the Workspace_User table a column named UserId as a foreign key.
        public User User { get; set; } = null!; // Navigation property

        [ForeignKey(nameof(WorkspaceId))]
        public Workspace Workspace { get; set; } = null!; // Navigation property

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } = null!; // Navigation property.

    }
}