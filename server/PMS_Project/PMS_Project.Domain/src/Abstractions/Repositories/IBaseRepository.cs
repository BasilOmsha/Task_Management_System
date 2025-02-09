namespace PMS_Project.Domain.Abstractions.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);

    }

}