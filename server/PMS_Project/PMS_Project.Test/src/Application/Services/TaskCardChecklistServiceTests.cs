using Moq;
using Xunit;
using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Application.Interfaces.Services;
using PMS_Project.Application.Services;
using AutoMapper;

namespace PMS_Project.Test.Application.Services
{
    public class TaskCardChecklistServiceTests
    {
        private readonly Mock<ITaskCardChecklistRepository> _taskCardChecklistRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ITaskCardChecklistService _taskCardChecklistService;

        public TaskCardChecklistServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskCardChecklist, TaskCardChecklistDTO>();
                cfg.CreateMap<TaskCardChecklistDTO, TaskCardChecklist>();
                cfg.CreateMap<TaskCardChecklistItem, TaskCardChecklistItemDTO>();
                cfg.CreateMap<TaskCardChecklistItemDTO, TaskCardChecklistItem>();
                // Add other mappings here
            });
            _mapper = config.CreateMapper();
            _taskCardChecklistRepositoryMock = new Mock<ITaskCardChecklistRepository>();
            _taskCardChecklistService = new TaskCardChecklistService(_taskCardChecklistRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTaskCardChecklist_WhenTaskCardChecklistExists()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            var checklist = new TaskCardChecklist { Id = checklistId, Title = "Test Checklist" };
            _taskCardChecklistRepositoryMock.Setup(repo => repo.GetByIdAsync(checklistId)).ReturnsAsync(checklist);

            // Act
            var result = await _taskCardChecklistService.GetByIdAsync(checklistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(checklistId, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTaskCardChecklists()
        {
            // Arrange
            var checklists = new List<TaskCardChecklist>
            {
                new TaskCardChecklist { Id = Guid.NewGuid(), Title = "Checklist 1" },
                new TaskCardChecklist { Id = Guid.NewGuid(), Title = "Checklist 2" }
            };
            _taskCardChecklistRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(checklists);

            // Act
            var result = await _taskCardChecklistService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_ShouldAddTaskCardChecklist()
        {
            // Arrange
            var checklistDTO = new TaskCardChecklistDTO { Id = Guid.NewGuid(), Title = "New Checklist" };

            // Act
            await _taskCardChecklistService.AddAsync(checklistDTO);

            // Assert
            _taskCardChecklistRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskCardChecklist>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTaskCardChecklistWithItems()
        {
            // Arrange
            var checklistDTO = new TaskCardChecklistDTO
            {
                Id = Guid.NewGuid(),
                Title = "New Checklist",
                Items = new List<TaskCardChecklistItemDTO>
                {
                    new TaskCardChecklistItemDTO { Id = Guid.NewGuid(), Name = "Item 1" },
                    new TaskCardChecklistItemDTO { Id = Guid.NewGuid(), Name = "Item 2" }
                }
            };

            // Act
            await _taskCardChecklistService.AddAsync(checklistDTO);

            // Assert
            _taskCardChecklistRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskCardChecklist>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTaskCardChecklist()
        {
            // Arrange
            var checklistDTO = new TaskCardChecklistDTO { Id = Guid.NewGuid(), Title = "Updated Checklist" };

            // Act
            await _taskCardChecklistService.UpdateAsync(checklistDTO.Id, checklistDTO);

            // Assert
            _taskCardChecklistRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskCardChecklist>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTaskCardChecklist()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            var checklist = new TaskCardChecklist { Id = checklistId, Title = "Test Checklist" };
            _taskCardChecklistRepositoryMock.Setup(repo => repo.GetByIdAsync(checklistId)).ReturnsAsync(checklist);

            // Act
            await _taskCardChecklistService.DeleteAsync(checklistId);

            // Assert
            _taskCardChecklistRepositoryMock.Verify(repo => repo.DeleteAsync(checklistId), Times.Once);
        }

        [Fact]
        public async Task GetByTaskCardIdAsync_ShouldReturnChecklists_WhenChecklistsExist()
        {
            // Arrange
            var taskCardId = Guid.NewGuid();
            var checklists = new List<TaskCardChecklist>
            {
                new TaskCardChecklist { Id = Guid.NewGuid(), Title = "Checklist 1", TaskCardId = taskCardId },
                new TaskCardChecklist { Id = Guid.NewGuid(), Title = "Checklist 2", TaskCardId = taskCardId }
            };
            _taskCardChecklistRepositoryMock.Setup(repo => repo.GetByTaskCardIdAsync(taskCardId)).ReturnsAsync(checklists);

            // Act
            var result = await _taskCardChecklistService.GetByTaskCardIdAsync(taskCardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}