using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IProjectBoardUserRepository : IBaseRepository<ProjectBoard_User>
    {
        Task<bool> GetProjectBoardUserAsync(Guid userId, Guid projectBoardId);
    }
}