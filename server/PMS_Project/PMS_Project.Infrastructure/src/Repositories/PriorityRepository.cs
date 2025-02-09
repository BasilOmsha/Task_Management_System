using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly PostgreSQLDbContext _context;
        public PriorityRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        /*public async Task GetTaskCardsByPriorityIdAsync(Guid priorityId)
        {
            // Implementation of the method
            var taskCards = await _context.TaskCard.Where(tc => tc.PriorityId == priorityId).ToListAsync();
            return taskCards;
        }*/

        public async Task<Priority> AddAsync(Priority entity)
        {
            await _context.Priority.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Priority> GetByIdAsync(Guid id)
        {
            return await _context.Priority.FindAsync(id);
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            return await _context.Priority.ToListAsync();
        }

        public async Task<Priority> UpdateAsync(Priority entity)
        {
            _context.Priority.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Priority.FindAsync(id);
            if (entity != null)
            {
                _context.Priority.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Priority> GetByNameAsync(string name)
        {
            return await _context.Priority
                .FirstOrDefaultAsync(p => p.Name == name) ?? new Priority();
        }
    }
}