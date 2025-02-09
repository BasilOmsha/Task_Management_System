using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Infrastructure.Database;

namespace PMS_Project.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly PostgreSQLDbContext _dbContext;
        public UserRepository(PostgreSQLDbContext _dbContext) : base(_dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _setDB.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _setDB.FirstOrDefaultAsync(u => u.Username == username);
        }


        // delete user
        public override async Task<bool> DeleteAsync(Guid id)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await _setDB.FindAsync(id);
                    if (entity == null)
                    {
                        return false;
                    }

                    _setDB.Remove(entity);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

    }
}