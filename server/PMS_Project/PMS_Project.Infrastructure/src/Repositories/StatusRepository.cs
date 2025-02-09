using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly PostgreSQLDbContext _context;

        public StatusRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        public async Task<Status> AddAsync(Status entity)
        {
            await _context.Status.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Status.FindAsync(id);
            if (entity != null)
            {
                _context.Status.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _context.Status.ToListAsync();
        }

        public async Task<Status> GetByIdAsync(Guid id)
        {
            return await _context.Status.FindAsync(id);
        }

        public async Task<Status> UpdateAsync(Status entity)
        {
            _context.Status.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Status> GetByNameAsync(string name)
        {
            var status = await _context.Status
                .FirstOrDefaultAsync(s => s.Name == name);
            if (status == null)
            {
                throw new KeyNotFoundException($"Status with name '{name}' not found.");
            }
            return status;
        }
    }
}