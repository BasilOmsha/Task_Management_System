using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {

        Task<Role> GetRoleByNameAsync(string roleName);
        
    }
}