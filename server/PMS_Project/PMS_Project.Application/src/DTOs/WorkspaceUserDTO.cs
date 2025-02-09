namespace PMS_Project.Application.DTOs
{
    public class GetWorkspaceUserDTO
    {
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        
    }

    public class CreateWorkspaceUserDTO
    {
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }

    public class UpdateWorkspaceUserDTO
    {
        public Guid RoleId { get; set; }

    }
}