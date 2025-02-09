namespace PMS_Project.Application.DTOs
{
    public class AppListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public Guid ProjectBoardId { get; set; }
        public ICollection<TaskCardDTO> TaskCard { get; set; }
    }

    public class CreateAppListDTO
    {
        public string Name { get; set; }
        public Guid ProjectBoardId { get; set; }
        public int Position { get; set; }
    }

    public class UpdateAppListDTO
    {
        public string Name { get; set; }
        public int Position { get; set; }
    }

    public class UpdateListPositionsDTO
    {
        public ICollection<ListPositionDTO> ListPositions { get; set; }
    }

    public class ListPositionDTO
    {
        public Guid ListId { get; set; }
        public int NewPosition { get; set; }
    }
}