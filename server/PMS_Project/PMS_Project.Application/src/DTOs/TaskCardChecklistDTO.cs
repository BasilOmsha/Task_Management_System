namespace PMS_Project.Application.DTOs
{
    public class TaskCardChecklistDTO
    {
        public Guid Id { get; set; }
        public Guid TaskCardId { get; set; }
        public string Title { get; set; }
        public ICollection<TaskCardChecklistItemDTO> Items { get; set; }
    }
}