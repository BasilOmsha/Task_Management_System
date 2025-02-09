using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class WorkspaceUserRepository : BaseRepository<Workspace_User>, IWorkspaceUserRepository
    {
        private readonly PostgreSQLDbContext _dbContext;
        public WorkspaceUserRepository(PostgreSQLDbContext dbContext) : base(dbContext)
        {
        }

        // public async Task<Role> GetUserRoleAsync(Guid Id)
        // {
        //    var workspaceUser = await _setDB.FirstOrDefaultAsync(wu => wu.RoleId == Id);
        //    if (workspaceUser == null)
        //        return null;

        //    return await _dbContext.Role.FirstOrDefaultAsync(r => r.Id == workspaceUser.RoleId);
        // }
        public async Task<Role> GetUserRoleAsync(Guid Id)
        {
            Console.WriteLine("!!!GetUserRoleAsync called with Id: {0}", Id);
            return await _dbContext.Role.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public async Task<Workspace_User> GetWorkspaceUserAsync(Guid uId, Guid wsId)
        {
            Console.WriteLine("!!!GetWorkspaceUserAsync called with userId: {0}, workspaceId: {1}", uId, wsId);
            if (uId != null && wsId != null)
            {
                Console.WriteLine("!All Good");
            }
            var workspaceUser = await _setDB.FirstOrDefaultAsync(wu => wu.UserId == uId && wu.WorkspaceId == wsId);
            if (workspaceUser != null)
            {
                Console.WriteLine("WorkspaceUser found");
            }
            else
            {
                Console.WriteLine("WorkspaceUser not found");
            }
            return await _setDB.FirstOrDefaultAsync(wu => wu.UserId == uId && wu.WorkspaceId == wsId);
        }

        public async Task<IEnumerable<Workspace_User>> GetAllUsers(Guid workspaceId)
        {
           if (workspaceId != null)
            {
                Console.WriteLine("!All Good");
            }
            var workspaceUsers = await _setDB.Where(wu => wu.WorkspaceId == workspaceId).ToListAsync();
            if (workspaceUsers != null)
            {
                Console.WriteLine("WorkspaceUsers found");
            }
            else
            {
                Console.WriteLine("WorkspaceUsers not found");
            }
            return await _setDB.Where(wu => wu.WorkspaceId == workspaceId).ToListAsync();
        }
    }
}