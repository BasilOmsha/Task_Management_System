namespace PMS_Project.Application.DTOs
{
    public class CreateProjectBoardUserDTO
    {
        public Guid ProjectBoardId { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetProjectBoardUserDTO
    {
        public Guid ProjectBoardId { get; set; }
        public Guid UserId { get; set; }
    }
}