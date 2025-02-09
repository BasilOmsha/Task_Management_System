using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Application.Interfaces.Services;
using PMS_Project.Domain.Models;
using AutoMapper;
using PMS_Project.Application.Abstractions.Services;

namespace PMS_Project.Application.Services
{
    public class TaskCardChecklistService : ITaskCardChecklistService, IBaseService<TaskCardChecklistDTO, TaskCardChecklistDTO, TaskCardChecklistDTO>
    {
        private readonly ITaskCardChecklistRepository _repository;
        private readonly IMapper _mapper;

        public TaskCardChecklistService(ITaskCardChecklistRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskCardChecklistDTO>> GetByTaskCardIdAsync(Guid taskCardId)
        {
            var checklists = await _repository.GetByTaskCardIdAsync(taskCardId);
            return _mapper.Map<IEnumerable<TaskCardChecklistDTO>>(checklists);
        }

        public async Task<TaskCardChecklistDTO> AddAsync(TaskCardChecklistDTO dto)
        {
            var checklist = _mapper.Map<TaskCardChecklist>(dto);
            await _repository.AddAsync(checklist);
            return _mapper.Map<TaskCardChecklistDTO>(checklist);
        }

        public async Task<TaskCardChecklistDTO> GetByIdAsync(Guid id)
        {
            var checklist = await _repository.GetByIdAsync(id);
            return _mapper.Map<TaskCardChecklistDTO>(checklist);
        }

        public async Task<IEnumerable<TaskCardChecklistDTO>> GetAllAsync()
        {
            var checklists = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskCardChecklistDTO>>(checklists);
        }

        public async Task<TaskCardChecklistDTO> UpdateAsync(Guid id, TaskCardChecklistDTO dto)
        {
            var checklist = _mapper.Map<TaskCardChecklist>(dto);
            checklist.Id = id;
            await _repository.UpdateAsync(checklist);
            return _mapper.Map<TaskCardChecklistDTO>(checklist);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        // ...other CRUD methods...
    }
}