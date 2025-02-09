namespace PMS_Project.Application.DTOs
{
    public class ProjectBoardLabelDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectBoardId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ICollection<Guid> TaskCardIds { get; set; } // New property
    }
}