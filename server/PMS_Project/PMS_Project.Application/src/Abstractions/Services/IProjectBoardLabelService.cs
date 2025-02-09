
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IProjectBoardLabelService : IBaseService<ProjectBoardLabelDTO, ProjectBoardLabelDTO, ProjectBoardLabelDTO>
    {
        /*Task<ProjectBoardLabelDTO> AddAsync(ProjectBoardLabelDTO projectBoardLabelDTO);
        Task<ProjectBoardLabelDTO> UpdateAsync(Guid id, ProjectBoardLabelDTO projectBoardLabelDTO);
        Task<bool> DeleteAsync(Guid id);
        Task<ProjectBoardLabelDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectBoardLabelDTO>> GetAllAsync();*/
    }
}