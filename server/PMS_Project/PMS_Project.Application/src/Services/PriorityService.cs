
using AutoMapper;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Services
{
    public class PriorityService : IBaseService<PriorityDTO, PriorityDTO, PriorityDTO>, IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        private readonly IMapper _mapper;

        public PriorityService(IPriorityRepository priorityRepository, IMapper mapper)
        {
            _priorityRepository = priorityRepository;
            _mapper = mapper;
        }

        public async Task<PriorityDTO> AddAsync(PriorityDTO dto)
        {
            // Implementation here
            var priority = _mapper.Map<Priority>(dto);
            await _priorityRepository.AddAsync(priority);
            return _mapper.Map<PriorityDTO>(priority);
        }

        public async Task<PriorityDTO> GetByIdAsync(Guid id)
        {
            // Implementation here
            var priority = await _priorityRepository.GetByIdAsync(id);
            return _mapper.Map<PriorityDTO>(priority);    
        }

        public async Task<IEnumerable<PriorityDTO>> GetAllAsync()
        {
            // Implementation here
            var priorities = await _priorityRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PriorityDTO>>(priorities);
        }

        public async Task<PriorityDTO> UpdateAsync(Guid id, PriorityDTO dto)
        {
            // Implementation here
            var priority = _mapper.Map<Priority>(dto);
            priority.Id = id;
            await _priorityRepository.UpdateAsync(priority);
            return _mapper.Map<PriorityDTO>(priority);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Implementation here
            return await _priorityRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TaskCardDTO>?> GetTaskCardsByPriorityIdAsync(Guid id)
        {
            // Implementation here
            var priority = await _priorityRepository.GetByIdAsync(id);
            return _mapper.Map<IEnumerable<TaskCardDTO>>(priority.TaskCards);
        }
    }
}