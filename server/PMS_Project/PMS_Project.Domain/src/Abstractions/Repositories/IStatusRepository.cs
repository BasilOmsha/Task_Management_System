using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IStatusRepository : IBaseRepository<Status>
    {
        // Additional methods specific to Status can be added here
        Task<Status> GetByNameAsync(string name);
    }
}