using PMS_Project.Domain.Models;
using PMS_Project.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class TaskCardActivityRepository : ITaskCardActivityRepository
    {
        private readonly PostgreSQLDbContext _context;

        public TaskCardActivityRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        public async Task<TaskCardActivity> AddAsync(TaskCardActivity entity)
        {
            await _context.TaskCardActivity.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TaskCardActivity> GetByIdAsync(Guid id)
        {
            return await _context.TaskCardActivity.FindAsync(id);
        }

        public async Task<IEnumerable<TaskCardActivity>> GetAllAsync()
        {
            return await _context.TaskCardActivity.ToListAsync();
        }

        public async Task<TaskCardActivity> UpdateAsync(TaskCardActivity entity)
        {
            _context.TaskCardActivity.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.TaskCardActivity.FindAsync(id);
            if (entity != null)
            {
                _context.TaskCardActivity.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<TaskCardActivity>> GetByTaskCardIdAsync(Guid taskCardId)
        {
            return await _context.TaskCardActivity.Where(a => a.TaskCardId == taskCardId).ToListAsync();
        }
    }
}