using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IWorkspaceUserRepository : IBaseRepository<Workspace_User>
    {
        Task<Workspace_User> GetWorkspaceUserAsync(Guid userId, Guid workspaceId);
        Task<Role> GetUserRoleAsync(Guid roleId);

        Task<IEnumerable<Workspace_User>> GetAllUsers(Guid workspaceId);
    }
}