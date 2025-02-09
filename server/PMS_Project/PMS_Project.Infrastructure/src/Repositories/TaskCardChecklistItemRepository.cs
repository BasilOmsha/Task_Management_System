using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class TaskCardChecklistItemRepository : ITaskCardChecklistItemRepository
    {
        private readonly PostgreSQLDbContext _context;

        public TaskCardChecklistItemRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        public async Task<TaskCardChecklistItem> AddAsync(TaskCardChecklistItem entity)
        {
            await _context.TaskCardChecklistItem.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TaskCardChecklistItem> GetByIdAsync(Guid id)
        {
            return await _context.TaskCardChecklistItem.FindAsync(id);
        }

        public async Task<IEnumerable<TaskCardChecklistItem>> GetAllAsync()
        {
            return await _context.TaskCardChecklistItem.ToListAsync();
        }

        public async Task<TaskCardChecklistItem> UpdateAsync(TaskCardChecklistItem entity)
        {
            _context.TaskCardChecklistItem.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.TaskCardChecklistItem.FindAsync(id);
            if (entity != null)
            {
                _context.TaskCardChecklistItem.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<TaskCardChecklistItem>> GetByChecklistIdAsync(Guid checklistId)
        {
            return await _context.TaskCardChecklistItem
                .Where(c => c.TaskCardChecklistId == checklistId)
                .ToListAsync();
        }
    }
}