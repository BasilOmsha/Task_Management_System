using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Interfaces.Repositories
{
    public interface ITaskCardChecklistItemRepository : IBaseRepository<TaskCardChecklistItem>
    {
        Task<IEnumerable<TaskCardChecklistItem>> GetByChecklistIdAsync(Guid checklistId);
    }
}