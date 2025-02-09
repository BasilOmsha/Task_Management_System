using PMS_Project.Application.Abstractions.Services;
using AutoMapper;
using PMS_Project.Domain.Abstractions.Repositories;

namespace PMS_Project.Application.Services
{
    public class BaseService<T, TCreateDTO, TGetDTO, TUpdateDTO> : IBaseService<TCreateDTO, TGetDTO, TUpdateDTO>
    {
        private readonly IBaseRepository<T> _repo;
        private readonly IMapper _mapper;

        public BaseService(IBaseRepository<T> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // The AddAsync method is used to create a new entity in the database.
        public virtual async Task<TGetDTO> AddAsync(TCreateDTO createDTO)
        {
            if (createDTO == null)
            {
                throw new ArgumentNullException(nameof(createDTO));
            }
            var userProperties = typeof(TCreateDTO).GetProperties();
            for (int i = 0; i < userProperties.Length; i++)
            {
                if (userProperties[i].GetValue(createDTO) == null)
                {
                    throw new ArgumentNullException(userProperties[i].Name);
                }
            }
            var newProperty = _mapper.Map<T>(createDTO);
            var newEntity = await _repo.AddAsync(newProperty);
            return _mapper.Map<TGetDTO>(newEntity);
        }

        public async Task<IEnumerable<TGetDTO>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TGetDTO>>(entities);
        }

        // The GetByIdAsync method is used to get a single entity from the database based on its id.
        public async Task<TGetDTO> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return _mapper.Map<TGetDTO>(entity);
        }

        public virtual async Task<TGetDTO> UpdateAsync(Guid id, TUpdateDTO updateDTO)
        {
            if (updateDTO == null)
            {
                throw new ArgumentNullException(nameof(updateDTO));
            }

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity not found.");
            }

            // Map the updateDTO onto the existing entity
            _mapper.Map(updateDTO, entity);

            // Await the asynchronous update operation
            var updatedEntity = await _repo.UpdateAsync(entity);

            // Map the updated entity to the GetDTO
            var mappedUser = _mapper.Map<TGetDTO>(updatedEntity);
            return mappedUser;
        }


        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return await _repo.DeleteAsync(id);
        }
    }
}