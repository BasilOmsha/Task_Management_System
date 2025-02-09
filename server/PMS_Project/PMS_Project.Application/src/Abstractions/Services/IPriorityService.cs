
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IPriorityService : IBaseService<PriorityDTO, PriorityDTO, PriorityDTO>
    {
        // Additional methods specific to Priority can be added here
        Task<IEnumerable<TaskCardDTO>?> GetTaskCardsByPriorityIdAsync(Guid priorityId);
    }
}