
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface ITaskCardActivityService : IBaseService<TaskCardActivityDTO, TaskCardActivityDTO, TaskCardActivityDTO>
    {
        Task<IEnumerable<TaskCardActivityDTO>> GetByTaskCardIdAsync(Guid taskCardId);
    }
}