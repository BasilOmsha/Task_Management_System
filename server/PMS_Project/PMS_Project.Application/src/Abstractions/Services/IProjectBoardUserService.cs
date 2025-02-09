using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IProjectBoardUserService : IBaseService<CreateProjectBoardUserDTO, GetProjectBoardUserDTO, CreateProjectBoardUserDTO>
    {
        Task<GetProjectBoardUserDTO> AddUserToProjectBoardAsync(CreateProjectBoardUserDTO createProjectBoardUserDTO, Guid currentUserId);
    }
}