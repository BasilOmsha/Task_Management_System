namespace PMS_Project.Application.DTOs
{
    public class TaskCardActivityDTO
    {
        public Guid Id { get; set; }
        public Guid TaskCardId { get; set; }
        public string Activity { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}