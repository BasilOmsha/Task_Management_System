using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IWorkspaceRepository : IBaseRepository<Workspace>
    {
        Task<Workspace?> GetByNameAndCreatorAsync(string name, Guid creatorUserId);

        Task<IEnumerable<Workspace>> GetWorkspacesForUserAsync(Guid userId);
    }
}