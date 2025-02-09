using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IWorkspaceUserService : IBaseService<CreateWorkspaceUserDTO, GetWorkspaceUserDTO, UpdateWorkspaceUserDTO>
    {
        // Task<GetWorkspaceUserDTO> AddOwnerUserToWorkspaceAsync(CreateWorkspaceUserDTO workspaceUserDTO);
        Task AddOwnerUserToWorkspaceAsync(Guid workspaceId, Guid userId, Guid roleId);

        Task<GetWorkspaceUserDTO> AddUserToWorkspaceAsync(CreateWorkspaceUserDTO createWorkspaceUserDTO, Guid currentUserId);

        Task<IEnumerable<GetWorkspaceUserDTO>> GetWorkspaceUsersAsync(Guid workspaceId);
    }
}