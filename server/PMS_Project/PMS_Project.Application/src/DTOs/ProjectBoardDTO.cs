namespace PMS_Project.Application.DTOs
{

    public class CreateProjectBoardDTO
    {
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
    public class ProjectBoardDTO
    {
        public Guid Id { get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid CreatorUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateProjectBoardDTO
    {
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}