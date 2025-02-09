using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class TaskCardChecklistRepository : ITaskCardChecklistRepository
    {
        private readonly PostgreSQLDbContext _context;

        public TaskCardChecklistRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }
    
        public async Task<TaskCardChecklist> AddAsync(TaskCardChecklist entity)
        {
            await _context.TaskCardChecklist.AddAsync(entity);
            if (entity.Items != null)
            {
                foreach (var item in entity.Items)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return entity;
        }
    
        public async Task<TaskCardChecklist> GetByIdAsync(Guid id)
        {
            return await _context.TaskCardChecklist.FindAsync(id);
        }
    
        public async Task<IEnumerable<TaskCardChecklist>> GetAllAsync()
        {
            return await _context.TaskCardChecklist.ToListAsync();
        }
    
        public async Task<TaskCardChecklist> UpdateAsync(TaskCardChecklist entity)
        {
            _context.TaskCardChecklist.Update(entity);
            if (entity.Items != null)
            {
                foreach (var item in entity.Items)
                {
                    _context.Entry(item).State = EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();
            return entity;
        }
    
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.TaskCardChecklist.FindAsync(id);
            if (entity != null)
            {
                _context.TaskCardChecklist.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<TaskCardChecklist>> GetByTaskCardIdAsync(Guid taskCardId)
        {
            return await _context.TaskCardChecklist
                .Where(c => c.TaskCardId == taskCardId)
                .ToListAsync();
        }
    }
}