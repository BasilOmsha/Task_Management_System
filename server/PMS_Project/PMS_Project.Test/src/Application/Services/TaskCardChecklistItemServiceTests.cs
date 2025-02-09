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
    public class TaskCardChecklistItemServiceTests
    {
        private readonly Mock<ITaskCardChecklistItemRepository> _taskCardChecklistItemRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ITaskCardChecklistItemService _taskCardChecklistItemService;

        public TaskCardChecklistItemServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskCardChecklistItem, TaskCardChecklistItemDTO>();
                cfg.CreateMap<TaskCardChecklistItemDTO, TaskCardChecklistItem>();
            });
            _mapper = config.CreateMapper();
            _taskCardChecklistItemRepositoryMock = new Mock<ITaskCardChecklistItemRepository>();
            _taskCardChecklistItemService = new TaskCardChecklistItemService(_taskCardChecklistItemRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTaskCardChecklistItem_WhenTaskCardChecklistItemExists()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var item = new TaskCardChecklistItem { Id = itemId, Name = "Test Item" };
            _taskCardChecklistItemRepositoryMock.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);

            // Act
            var result = await _taskCardChecklistItemService.GetByIdAsync(itemId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(itemId, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTaskCardChecklistItems()
        {
            // Arrange
            var items = new List<TaskCardChecklistItem>
            {
                new TaskCardChecklistItem { Id = Guid.NewGuid(), Name = "Item 1" },
                new TaskCardChecklistItem { Id = Guid.NewGuid(), Name = "Item 2" }
            };
            _taskCardChecklistItemRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _taskCardChecklistItemService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_ShouldAddTaskCardChecklistItem()
        {
            // Arrange
            var itemDTO = new TaskCardChecklistItemDTO { Id = Guid.NewGuid(), Name = "New Item" };

            // Act
            await _taskCardChecklistItemService.AddAsync(itemDTO);

            // Assert
            _taskCardChecklistItemRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskCardChecklistItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTaskCardChecklistItem()
        {
            // Arrange
            var itemDTO = new TaskCardChecklistItemDTO { Id = Guid.NewGuid(), Name = "Updated Item" };

            // Act
            await _taskCardChecklistItemService.UpdateAsync(itemDTO.Id, itemDTO);

            // Assert
            _taskCardChecklistItemRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskCardChecklistItem>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTaskCardChecklistItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var item = new TaskCardChecklistItem { Id = itemId, Name = "Test Item" };
            _taskCardChecklistItemRepositoryMock.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);

            // Act
            await _taskCardChecklistItemService.DeleteAsync(itemId);

            // Assert
            _taskCardChecklistItemRepositoryMock.Verify(repo => repo.DeleteAsync(itemId), Times.Once);
        }

        [Fact]
        public async Task GetByChecklistIdAsync_ShouldReturnItems_WhenItemsExist()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            var items = new List<TaskCardChecklistItem>
            {
                new TaskCardChecklistItem { Id = Guid.NewGuid(), Name = "Item 1", TaskCardChecklistId = checklistId },
                new TaskCardChecklistItem { Id = Guid.NewGuid(), Name = "Item 2", TaskCardChecklistId = checklistId }
            };
            _taskCardChecklistItemRepositoryMock.Setup(repo => repo.GetByChecklistIdAsync(checklistId)).ReturnsAsync(items);

            // Act
            var result = await _taskCardChecklistItemService.GetByChecklistIdAsync(checklistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}