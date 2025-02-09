using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Interfaces.Services
{
    public interface ITaskCardChecklistItemService : IBaseService<TaskCardChecklistItemDTO, TaskCardChecklistItemDTO, TaskCardChecklistItemDTO>
    {
        Task<IEnumerable<TaskCardChecklistItemDTO>> GetByChecklistIdAsync(Guid checklistId);
    }
}