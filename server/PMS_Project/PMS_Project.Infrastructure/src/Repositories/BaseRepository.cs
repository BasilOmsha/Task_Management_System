using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private const int DefaultOffset = 0;
        private const int DefaultLimit = 10;
        protected readonly PostgreSQLDbContext _dbContext;
        protected readonly DbSet<T> _setDB;


        public BaseRepository(PostgreSQLDbContext dbContext)
        {
            _dbContext = dbContext;
            _setDB = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _setDB.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _setDB.Skip(DefaultOffset).Take(DefaultLimit).ToListAsync();
            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _setDB.FindAsync(id);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _setDB.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _setDB.FindAsync(id);
            _setDB.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}