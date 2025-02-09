using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IProjectBoardService : IBaseService<CreateProjectBoardDTO, ProjectBoardDTO, UpdateProjectBoardDTO>
    {
        /*Task<ProjectBoardDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectBoardDTO>> GetAllAsync();
        Task AddAsync(ProjectBoardDTO projectBoard);
        Task UpdateAsync(ProjectBoardDTO projectBoard);
        Task DeleteAsync(Guid id);*/
        Task<ProjectBoardDTO> UpdateAsync(Guid id, UpdateProjectBoardDTO projectBoardDTO);
        Task<ProjectBoardDTO> CreateNewProjectAsync(CreateProjectBoardDTO createProjectBoard, Guid userId);
        Task<IEnumerable<ProjectBoardDTO>> GetProjectsByWorkspaceIdAsync(Guid workspaceId, Guid userId);
    }
}