
using PMS_Project.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface ITaskCardRepository : IBaseRepository<TaskCard>
    {
        Task AddTaskCardUserAsync(TaskCard_User taskCardUser);
        Task RemoveTaskCardUserAsync(Guid taskCardId, Guid userId);

        Task<IEnumerable<TaskCard>> GetTaskCardsByListAsync(Guid listId);
        Task UpdateTaskCardPositionsAsync(IEnumerable<TaskCard> taskCards);

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}