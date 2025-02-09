using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Interfaces.Services
{
    public interface ITaskCardChecklistService : IBaseService<TaskCardChecklistDTO, TaskCardChecklistDTO, TaskCardChecklistDTO>
    {
        Task<IEnumerable<TaskCardChecklistDTO>> GetByTaskCardIdAsync(Guid taskCardId);
        Task<TaskCardChecklistDTO> AddAsync(TaskCardChecklistDTO dto);
        // ...other CRUD methods...
    }
}