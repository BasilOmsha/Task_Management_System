using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IWorkspaceService : IBaseService<CreateWorkspaceDTO, GetWorkspaceDTO, UpdateWorkspaceDTO>
    {
        Task<IEnumerable<GetWorkspaceDTO>> GetAllAsync(Guid userId);
        Task AddProjectBoardAsync(Guid workspaceId, ProjectBoard projectBoard);
        Task RemoveProjectBoardAsync(Guid workspaceId, ProjectBoard projectBoard);
        Task<GetWorkspaceDTO> AddWorkspaceAsync(CreateWorkspaceDTO workspaceDto, Guid userId);
    }
}