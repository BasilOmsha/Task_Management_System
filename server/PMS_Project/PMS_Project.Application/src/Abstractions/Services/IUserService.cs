using System.Data.Common;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IUserService : IBaseService<CreateUserDTO, GetUserInfoDTO, UpdateUserInfoDTO>
    {
        Task<bool> CheckUserEmailExistsAsync(string email);
        Task<bool> CheckUserUsernameExistsAsync(string username);
        Task AddTaskCardActivityAsync(Guid userId, TaskCardActivity taskCard);
        Task RemoveTaskCardActivityAsync(Guid userId, TaskCardActivity taskCard);
        Task<GetUserInfoDTO> GetUserByEmailAsync(string email);

    }
}