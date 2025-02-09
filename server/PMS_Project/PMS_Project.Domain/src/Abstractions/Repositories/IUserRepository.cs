using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUserNameAsync(string username);
    }
}