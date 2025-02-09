using System.ComponentModel.DataAnnotations;

namespace PMS_Project.Application.DTOs
{

    public class CreateRoleDTO
    {
        [Required, MaxLength(30, ErrorMessage = "Role name must be at most 30 characters long.")]
        public string? Name { get; set; }

        [MaxLength(255, ErrorMessage = "Role description must be at most 255 characters long.")]
        public string? Description { get; set; }
        
    }

    public class GetRoleDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }

    public class UpdateRoleDTO
    {
        [MaxLength(30, ErrorMessage = "Role name must be at most 30 characters long.")]
        public string? Name { get; set; }

        [MaxLength(255, ErrorMessage = "Role description must be at most 255 characters long.")]
        public string? Description { get; set; }

    }

    public class AssignRoleDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        // Optional: Specify context (Workspace or Project)
        public Guid? WorkspaceId { get; set; }
        public Guid? ProjectBoardId { get; set; }
    }

    // Enumeration of the possible roles
    public enum RoleEnum
    {
        Owner = 1,
        Admin = 2,
        Contributor = 3,
    }
}