using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class ProjectBoardUserRepository : BaseRepository<ProjectBoard_User>, IProjectBoardUserRepository
    {
        private readonly PostgreSQLDbContext _dbContext;
        public ProjectBoardUserRepository(PostgreSQLDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> GetProjectBoardUserAsync(Guid userId, Guid projectBoardId)
        {
            //validate variables
            if (userId == Guid.Empty || projectBoardId == Guid.Empty)
            {
                throw new ArgumentNullException("userId or projectBoardId is empty");
            }

            var projectBoardUser = await _setDB.FirstOrDefaultAsync(pbu => pbu.UserId == userId && pbu.ProjectBoardId == projectBoardId);
            if (projectBoardUser != null)
            {
                return false;
            }
            return true;

        }
    }
}