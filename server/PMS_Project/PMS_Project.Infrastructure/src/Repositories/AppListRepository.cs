using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Domain.src.Abstractions.Repositories;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class AppListRepository : IAppListRepository
    {
        private readonly PostgreSQLDbContext _context;

        public AppListRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        public async Task<AppList> AddAsync(AppList entity)
        {
            await _context.AppList.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<AppList> GetByIdAsync(Guid id)
        {
            return await _context.AppList
                .Include(l => l.TaskCard.OrderBy(tc => tc.Position))
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<AppList>> GetAllAsync()
        {
            return await _context.AppList
                .Include(l => l.TaskCard.OrderBy(tc => tc.Position))
                .OrderBy(l => l.Position)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppList>> GetListsByProjectBoardAsync(Guid projectBoardId)
        {
            return await _context.AppList
                .Where(l => l.ProjectBoardId == projectBoardId)
                .Include(l => l.TaskCard.OrderBy(tc => tc.Position))
                .OrderBy(l => l.Position)
                .ToListAsync();
        }

        public async Task UpdateListPositionsAsync(IEnumerable<AppList> lists)
        {
            foreach (var list in lists)
            {
                _context.Entry(list).Property(x => x.Position).IsModified = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<AppList> UpdateAsync(AppList entity)
        {
            _context.AppList.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.AppList.FindAsync(id);
            if (entity != null)
            {
                _context.AppList.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}