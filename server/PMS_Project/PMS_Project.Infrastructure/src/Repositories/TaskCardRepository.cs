using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class TaskCardRepository : ITaskCardRepository
    {
        private readonly PostgreSQLDbContext _context;

        public TaskCardRepository(PostgreSQLDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<TaskCard> AddAsync(TaskCard entity)
        {
            try
            {
                // Initialize collections if they're null
                entity.Activities ??= new List<TaskCardActivity>();
                entity.TaskCard_ProjectBoardLabels ??= new List<TaskCard_ProjectBoardLabel>();
                entity.TaskCard_Users ??= new List<TaskCard_User>();

                await _context.TaskCard.AddAsync(entity);
                await _context.SaveChangesAsync();

                // Reload the entity with relationships
                return await _context.TaskCard
                    .Include(tc => tc.Activities)
                    .Include(tc => tc.Status)
                    .FirstOrDefaultAsync(tc => tc.Id == entity.Id) ?? throw new InvalidOperationException("TaskCard not found after adding.");
            }
            catch (Exception ex)
            {
                // Log the exception details here
                throw;
            }
        }

        public async Task<TaskCard> GetByIdAsync(Guid id)
        {
            return await _context.TaskCard
                .Include(tc => tc.Activities)
                .Include(tc => tc.Status)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<IEnumerable<TaskCard>> GetAllAsync()
        {
            return await _context.TaskCard
                .Include(tc => tc.Activities)
                .Include(tc => tc.Status)
                .OrderBy(tc => tc.Position)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskCard>> GetTaskCardsByListAsync(Guid listId)
        {
            return await _context.TaskCard
                .Where(tc => tc.ListId == listId)
                .Include(tc => tc.Activities)
                .Include(tc => tc.Status)
                .OrderBy(tc => tc.Position)
                .ToListAsync();
        }

        public async Task UpdateTaskCardPositionsAsync(IEnumerable<TaskCard> taskCards)
        {
            foreach (var taskCard in taskCards)
            {
                _context.Entry(taskCard).Property(x => x.Position).IsModified = true;
                _context.Entry(taskCard).Property(x => x.ListId).IsModified = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<TaskCard> UpdateAsync(TaskCard entity)
        {
            _context.TaskCard.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.TaskCard.FindAsync(id);
            if (entity != null)
            {
                _context.TaskCard.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AddTaskCardUserAsync(TaskCard_User taskCardUser)
        {
            await _context.TaskCard_User.AddAsync(taskCardUser);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTaskCardUserAsync(Guid taskCardId, Guid userId)
        {
            var taskCardUser = await _context.TaskCard_User
                .FirstOrDefaultAsync(tcu => tcu.TaskCardId == taskCardId && tcu.UserId == userId);
            if (taskCardUser != null)
            {
                _context.TaskCard_User.Remove(taskCardUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}