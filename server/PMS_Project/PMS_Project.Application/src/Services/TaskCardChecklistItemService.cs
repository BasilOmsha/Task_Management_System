using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Application.Interfaces.Services;
using PMS_Project.Domain.Models;
using AutoMapper;
using PMS_Project.Application.Abstractions.Services;

namespace PMS_Project.Application.Services
{
    public class TaskCardChecklistItemService : ITaskCardChecklistItemService, IBaseService<TaskCardChecklistItemDTO, TaskCardChecklistItemDTO, TaskCardChecklistItemDTO>
    {
        private readonly ITaskCardChecklistItemRepository _repository;
        private readonly IMapper _mapper;

        public TaskCardChecklistItemService(ITaskCardChecklistItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskCardChecklistItemDTO>> GetByChecklistIdAsync(Guid checklistId)
        {
            var items = await _repository.GetByChecklistIdAsync(checklistId);
            return _mapper.Map<IEnumerable<TaskCardChecklistItemDTO>>(items);
        }

        public async Task<TaskCardChecklistItemDTO> AddAsync(TaskCardChecklistItemDTO dto)
        {
            var item = _mapper.Map<TaskCardChecklistItem>(dto);
            await _repository.AddAsync(item);
            return _mapper.Map<TaskCardChecklistItemDTO>(item);
        }

        public async Task<TaskCardChecklistItemDTO> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<TaskCardChecklistItemDTO>(item);
        }

        public async Task<IEnumerable<TaskCardChecklistItemDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskCardChecklistItemDTO>>(items);
        }

        public async Task<TaskCardChecklistItemDTO> UpdateAsync(Guid id, TaskCardChecklistItemDTO dto)
        {
            var item = _mapper.Map<TaskCardChecklistItem>(dto);
            item.Id = id;
            await _repository.UpdateAsync(item);
            return _mapper.Map<TaskCardChecklistItemDTO>(item);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}