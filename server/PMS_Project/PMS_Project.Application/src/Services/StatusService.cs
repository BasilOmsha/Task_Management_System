using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using AutoMapper;
using PMS_Project.Application.Abstractions.Services;

namespace PMS_Project.Application.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repository;
        private readonly IMapper _mapper;

        public StatusService(IStatusRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<StatusDTO> AddAsync(StatusDTO dto)
        {
            var status = _mapper.Map<Status>(dto);
            await _repository.AddAsync(status);
            return _mapper.Map<StatusDTO>(status);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StatusDTO>> GetAllAsync()
        {
            var statuses = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<StatusDTO>>(statuses);
        }

        public async Task<StatusDTO> GetByIdAsync(Guid id)
        {
            var status = await _repository.GetByIdAsync(id);
            return _mapper.Map<StatusDTO>(status);
        }

        public async Task<StatusDTO> UpdateAsync(Guid id, StatusDTO dto)
        {
            var status = _mapper.Map<Status>(dto);
            status.Id = id;
            await _repository.UpdateAsync(status);
            return _mapper.Map<StatusDTO>(status);
        }

        public async Task<IEnumerable<TaskCardDTO>?> GetTaskCardsByStatusIdAsync(Guid statusId)
        {
            var status = await _repository.GetByIdAsync(statusId);
            return _mapper.Map<IEnumerable<TaskCardDTO>>(status.TaskCards);
        }
    }
}