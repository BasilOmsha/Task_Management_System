using PMS_Project.Domain.Models;

namespace PMS_Project.Application.DTOs
{

    public class WorkspaceUserDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class CreateWorkspaceDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
    public class GetWorkspaceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CreatorUserId { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatorUsername { get; set; }
        public ICollection<WorkspaceUserDTO> WorkspaceUsers { get; set; }
    }

    public class UpdateWorkspaceDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}