using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Interfaces.Repositories
{
    public interface ITaskCardChecklistRepository : IBaseRepository<TaskCardChecklist>
    {
        Task<IEnumerable<TaskCardChecklist>> GetByTaskCardIdAsync(Guid taskCardId);
    }
}