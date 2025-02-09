
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface ITaskCardService : IBaseService<CreateTaskCardDTO, TaskCardDTO, TaskCardDTO>
    {
        Task<TaskCardDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskCardDTO>> GetAllAsync();
        Task<TaskCardDTO> AddAsync(CreateTaskCardDTO taskCardDTO);
        Task<TaskCardDTO> UpdateAsync(Guid id, TaskCardDTO taskCardDTO);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TaskCardDTO>> GetTaskCardsByListAsync(Guid listId);
        Task UpdateTaskCardPositionAsync(Guid taskCardId, Guid newListId, int newPosition);
        Task UpdateMultipleTaskCardPositionsAsync(IEnumerable<TaskCardPositionUpdateDTO> positionUpdates);
        Task AddTaskCardActivityAsync(Guid taskCardId, TaskCardActivity activity);
        Task RemoveTaskCardActivityAsync(Guid taskCardId, TaskCardActivity activity);
        Task AddUserToTaskCardAsync(Guid taskCardId, Guid userId);
        Task RemoveUserFromTaskCardAsync(Guid taskCardId, Guid userId);

    }
}