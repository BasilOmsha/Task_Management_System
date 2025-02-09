using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IProjectBoardRepository : IBaseRepository<ProjectBoard>
    {
        // No additional methods needed as they are inherited from IBaseRepository

        Task<bool> GetProjectBoardByNameAsync(string name, Guid workspaceId);

        Task<IEnumerable<ProjectBoard>> GetProjectsByWorkspaceIdAsync(Guid workspaceId);
    }
}