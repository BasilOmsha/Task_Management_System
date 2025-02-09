namespace PMS_Project.Application.DTOs
{
    public class TaskCardPositionUpdateDTO
    {
        public Guid TaskCardId { get; set; }
        public Guid ListId { get; set; }
        public int Position { get; set; }
    }
    public class TaskCardDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid ListId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public Guid PriorityId { get; set; }
        public string DueDate { get; set; }
        public Guid StatusId { get; set; }
        public int Position { get; set; }
        public ICollection<TaskCardActivityDTO> Activities { get; set; }
        public StatusDTO Status { get; set; }
        public ICollection<ProjectBoardLabelDTO> Labels { get; set; } // New property
    }

    public class CreateTaskCardDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ListId { get; set; }
        public int Position { get; set; }
    }
}