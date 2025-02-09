using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Application.Interfaces.Services;
using PMS_Project.Domain.Models;
using AutoMapper;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Domain.Abstractions.Repositories;

namespace PMS_Project.Application.Services
{
    public class TaskCardActivityService : ITaskCardActivityService, IBaseService<TaskCardActivityDTO, TaskCardActivityDTO, TaskCardActivityDTO>
    {
        private readonly ITaskCardActivityRepository _repository;
        private readonly IMapper _mapper;

        public TaskCardActivityService(ITaskCardActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskCardActivityDTO>> GetByTaskCardIdAsync(Guid taskCardId)
        {
            var activities = await _repository.GetByTaskCardIdAsync(taskCardId);
            return _mapper.Map<IEnumerable<TaskCardActivityDTO>>(activities);
        }

        public async Task<TaskCardActivityDTO> AddAsync(TaskCardActivityDTO dto)
        {
            var activity = _mapper.Map<TaskCardActivity>(dto);
            activity.TaskCardId = dto.TaskCardId; // Ensure TaskCardId is set
            await _repository.AddAsync(activity);
            return _mapper.Map<TaskCardActivityDTO>(activity);
        }

        public async Task<TaskCardActivityDTO> GetByIdAsync(Guid id)
        {
            var activity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TaskCardActivityDTO>(activity);
        }

        public async Task<IEnumerable<TaskCardActivityDTO>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskCardActivityDTO>>(activities);
        }

        public async Task<TaskCardActivityDTO> UpdateAsync(Guid id, TaskCardActivityDTO dto)
        {
            var activity = _mapper.Map<TaskCardActivity>(dto);
            activity.Id = id;
            await _repository.UpdateAsync(activity);
            return _mapper.Map<TaskCardActivityDTO>(activity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}