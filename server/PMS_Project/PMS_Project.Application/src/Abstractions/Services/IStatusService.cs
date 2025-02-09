using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IStatusService : IBaseService<StatusDTO, StatusDTO, StatusDTO>
    {
        // Additional methods specific to Status can be added here
        Task<IEnumerable<TaskCardDTO>?> GetTaskCardsByStatusIdAsync(Guid statusId);
    }
}