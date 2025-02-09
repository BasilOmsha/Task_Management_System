using PMS_Project.Domain.Models;
using PMS_Project.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
    {
        private readonly PostgreSQLDbContext _context;

        public WorkspaceRepository(PostgreSQLDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Workspace?> GetByNameAndCreatorAsync(string name, Guid creatorUserId)
        {
            return await _context.Workspace
                .FirstOrDefaultAsync(w =>
                    w.CreatorUserId == creatorUserId &&
                    w.Name.ToLower() == name.ToLower());
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var workspace = await _context.Workspace.FindAsync(id);
            if (workspace != null)
            {
                _context.Workspace.Remove(workspace);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Workspace>> GetWorkspacesForUserAsync(Guid userId)
        {
            return await _context.Workspace
                .Include(w => w.User)
                .Include(w => w.Workspace_Users)
                    .ThenInclude(wu => wu.User)
                .Include(w => w.Workspace_Users)
                    .ThenInclude(wu => wu.Role)
                .Where(w =>
                    w.CreatorUserId == userId || // User owns the workspace
                    w.Workspace_Users.Any(wu => wu.UserId == userId)  //|| // User is a member
                    // (w.IsPublic && !w.Workspace_Users.Any(wu => wu.UserId == userId)) // Workspace is public and user is not a member
                )
                .ToListAsync();
        }
    }
}