using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class User : BaseClass
    {
        [Required]
        [MaxLength(100)]
        public string? Firstname { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Lastname { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public bool TokenRevoked { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public ICollection<Workspace>? CreatedWorkspaces { get; set; } // One-to-many with workspaces - navigation property
        public ICollection<ProjectBoard>? ProjectBoards { get; set; } // One-to-many with ProjectBoard - navigation property
        public ICollection<TaskCardActivity>? TaskCardActivities { get; set; } // One-to-many with task card activities - navigation property
        public ICollection<TaskCard_User>? TaskCard_Users { get; set; } // Navigation property for many-to-many relationship with TaskCard
        public ICollection<Workspace_User>? Workspace_Users { get; set; } // Navigation property for many-to-many relationship with Workspace
        public ICollection<ProjectBoard_User>? ProjectBoard_Users { get; set; } // Navigation property for many-to-many relationship with ProjectBoard
    }
}