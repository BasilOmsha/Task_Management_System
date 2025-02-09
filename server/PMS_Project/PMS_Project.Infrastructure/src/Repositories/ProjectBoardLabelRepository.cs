
using PMS_Project.Domain.Models;
using PMS_Project.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class ProjectBoardLabelRepository : IProjectBoardLabelRepository
    {
        private readonly PostgreSQLDbContext _context;

        public ProjectBoardLabelRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectBoardLabel> GetByIdAsync(Guid id)
        {
            return await _context.ProjectBoardLabel.FindAsync(id);
        }

        public async Task<IEnumerable<ProjectBoardLabel>> GetAllAsync()
        {
            return await _context.ProjectBoardLabel.ToListAsync();
        }

        public async Task<ProjectBoardLabel> AddAsync(ProjectBoardLabel projectBoardLabel)
        {
            await _context.ProjectBoardLabel.AddAsync(projectBoardLabel);
            await _context.SaveChangesAsync();
            return projectBoardLabel;
        }

        public async Task<ProjectBoardLabel> UpdateAsync(ProjectBoardLabel projectBoardLabel)
        {
            _context.ProjectBoardLabel.Update(projectBoardLabel);
            await _context.SaveChangesAsync();
            return projectBoardLabel;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var projectBoardLabel = await _context.ProjectBoardLabel.FindAsync(id);
            if (projectBoardLabel != null)
            {
                _context.ProjectBoardLabel.Remove(projectBoardLabel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}