namespace PMS_Project.Application.Abstractions.Services
{
    public interface IBaseService<TCreateDTO, TGetDTO, TUpdateDTO>
    {
        Task<TGetDTO> AddAsync(TCreateDTO createDTO); // Create new entity
        Task<TGetDTO> GetByIdAsync(Guid id); // Get one entity by id
        Task<IEnumerable<TGetDTO>> GetAllAsync(); // Get all entities
        Task<TGetDTO> UpdateAsync(Guid id, TUpdateDTO updateDTO); // Update one entity by id
        Task<bool> DeleteAsync(Guid id); // Delete one entity by id
        

    }
}