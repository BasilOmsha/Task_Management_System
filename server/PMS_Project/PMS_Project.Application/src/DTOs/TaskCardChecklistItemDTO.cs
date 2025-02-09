
namespace PMS_Project.Application.DTOs
{
    public class TaskCardChecklistItemDTO
    {
        public Guid Id { get; set; }
        public Guid TaskCardChecklistId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public int Position { get; set; }
    }
}