using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using System.Text.RegularExpressions;

namespace PMS_Project.Application.Services
{
    public class ProjectBoardLabelService : IProjectBoardLabelService
    {
        private readonly IProjectBoardLabelRepository _projectBoardLabelRepository;

        public ProjectBoardLabelService(IProjectBoardLabelRepository projectBoardLabelRepository)
        {
            _projectBoardLabelRepository = projectBoardLabelRepository;
        }

        public async Task<ProjectBoardLabelDTO> AddAsync(ProjectBoardLabelDTO projectBoardLabelDTO)
        {
            ValidateColor(projectBoardLabelDTO.Color);
            var projectBoardLabel = new ProjectBoardLabel
            {
                Id = projectBoardLabelDTO.Id,
                ProjectBoardId = projectBoardLabelDTO.ProjectBoardId,
                Name = projectBoardLabelDTO.Name,
                Color = projectBoardLabelDTO.Color,
                TaskCard_ProjectBoardLabels = projectBoardLabelDTO.TaskCardIds.Select(taskCardId => new TaskCard_ProjectBoardLabel
                {
                    TaskCardId = taskCardId,
                    ProjectBoardLabelId = projectBoardLabelDTO.Id
                }).ToList()
            };
            await _projectBoardLabelRepository.AddAsync(projectBoardLabel);
            return projectBoardLabelDTO;
        }

        public async Task<ProjectBoardLabelDTO> UpdateAsync(Guid id, ProjectBoardLabelDTO projectBoardLabelDTO)
        {
            ValidateColor(projectBoardLabelDTO.Color);
            var projectBoardLabel = new ProjectBoardLabel
            {
                Id = projectBoardLabelDTO.Id,
                ProjectBoardId = projectBoardLabelDTO.ProjectBoardId,
                Name = projectBoardLabelDTO.Name,
                Color = projectBoardLabelDTO.Color,
                TaskCard_ProjectBoardLabels = projectBoardLabelDTO.TaskCardIds.Select(taskCardId => new TaskCard_ProjectBoardLabel
                {
                    TaskCardId = taskCardId,
                    ProjectBoardLabelId = projectBoardLabelDTO.Id
                }).ToList()
            };
            await _projectBoardLabelRepository.UpdateAsync(projectBoardLabel);
            return projectBoardLabelDTO;
        }

        private void ValidateColor(string color)
        {
            if (!Regex.IsMatch(color, "^#(?:[0-9a-fA-F]{3}){1,2}$"))
            {
                throw new ArgumentException("Invalid color hex code.");
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _projectBoardLabelRepository.DeleteAsync(id);
        }

        public async Task<ProjectBoardLabelDTO> GetByIdAsync(Guid id)
        {
            var projectBoardLabel = await _projectBoardLabelRepository.GetByIdAsync(id);
            return new ProjectBoardLabelDTO
            {
                Id = projectBoardLabel.Id,
                ProjectBoardId = projectBoardLabel.ProjectBoardId,
                Name = projectBoardLabel.Name,
                Color = projectBoardLabel.Color
            };
        }

        public async Task<IEnumerable<ProjectBoardLabelDTO>> GetAllAsync()
        {
            var projectBoardLabels = await _projectBoardLabelRepository.GetAllAsync();
            return projectBoardLabels.Select(pbl => new ProjectBoardLabelDTO
            {
                Id = pbl.Id,
                ProjectBoardId = pbl.ProjectBoardId,
                Name = pbl.Name,
                Color = pbl.Color
            });
        }
    }
}