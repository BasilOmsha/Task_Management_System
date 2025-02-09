
using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IPriorityRepository : IBaseRepository<Priority>
    {
        // Additional methods specific to Priority can be added here
        //Task GetTaskCardsByPriorityIdAsync(Guid id);
        Task<Priority> GetByNameAsync(string name);
    }
}