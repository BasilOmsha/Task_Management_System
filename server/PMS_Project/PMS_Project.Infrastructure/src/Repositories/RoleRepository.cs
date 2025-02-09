using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly PostgreSQLDbContext _dbContext;
        public RoleRepository(PostgreSQLDbContext _dbContext) : base(_dbContext)
        {

        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _setDB.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
        }

    }
}