using PMS_Project.Domain.Models;
using PMS_Project.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class ProjectBoardRepository : BaseRepository<ProjectBoard>, IProjectBoardRepository
    {
        private readonly PostgreSQLDbContext _context;

        public ProjectBoardRepository(PostgreSQLDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProjectBoard>> GetProjectsByWorkspaceIdAsync(Guid workspaceId)
        {
            return await _setDB
                .Where(pb => pb.WorkspaceId == workspaceId)
                .ToListAsync();
        }
        public async Task<bool> GetProjectBoardByNameAsync(string name, Guid workspaceId)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Project board name cannot be null or empty", nameof(name));
                }

                if (workspaceId == Guid.Empty)
                {
                    throw new ArgumentException("Workspace ID cannot be an empty GUID", nameof(workspaceId));
                }

                Console.WriteLine("!!! GetProjectBoardByNameAsync called with name: {0}, workspaceId: {1}", name, workspaceId);
                // Query the database for the project board within the specified workspace
                var projectBoard = await _setDB
                    .FirstOrDefaultAsync(pb => pb.Name == name && pb.WorkspaceId == workspaceId);

                if (projectBoard != null)
                {
                    Console.WriteLine("!!! Project board found: {0}", projectBoard.Name);
                    return false;
                }
                else
                {
                    Console.WriteLine("!!! Project board not found");
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("!!! Exception in GetProjectBoardByNameAsync: {0}", ex.Message);
                throw; // Optionally re-throw for higher-level error handling
            }
        }

    }
}