using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface ITaskCardActivityRepository : IBaseRepository<TaskCardActivity>
    {
        Task<IEnumerable<TaskCardActivity>> GetByTaskCardIdAsync(Guid taskCardId);
    }
}