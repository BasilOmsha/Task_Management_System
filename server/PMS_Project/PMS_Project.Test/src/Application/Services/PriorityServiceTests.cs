using AutoMapper;
using Moq;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Services;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using Xunit;

namespace PMS_Project.Test.Application.Services
{
    public class PriorityServiceTests
    {
        private readonly Mock<IPriorityRepository> _priorityRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IPriorityService _priorityService;

        public PriorityServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Priority, PriorityDTO>();
                cfg.CreateMap<PriorityDTO, Priority>();
                cfg.CreateMap<TaskCard, TaskCardDTO>();
            });
            _mapper = config.CreateMapper();
            _priorityRepositoryMock = new Mock<IPriorityRepository>();
            _priorityService = new PriorityService(_priorityRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task AddAsync_ShouldAddPriority()
        {
            // Arrange
            var priorityDTO = new PriorityDTO { Name = "High" };
            var priority = new Priority { Id = Guid.NewGuid(), Name = "High" };
            _priorityRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Priority>())).ReturnsAsync(priority);

            // Act
            var result = await _priorityService.AddAsync(priorityDTO);

            // Assert
            Assert.Equal(priorityDTO.Name, result.Name);
            _priorityRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Priority>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPriority_WhenPriorityExists()
        {
            // Arrange
            var priorityId = Guid.NewGuid();
            var priority = new Priority { Id = priorityId, Name = "Medium" };
            _priorityRepositoryMock.Setup(repo => repo.GetByIdAsync(priorityId)).ReturnsAsync(priority);

            // Act
            var result = await _priorityService.GetByIdAsync(priorityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(priorityId, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPriorities()
        {
            // Arrange
            var priorities = new List<Priority>
            {
                new Priority { Id = Guid.NewGuid(), Name = "Low" },
                new Priority { Id = Guid.NewGuid(), Name = "High" }
            };
            _priorityRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(priorities);

            // Act
            var result = await _priorityService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePriority()
        {
            // Arrange
            var priorityDTO = new PriorityDTO { Id = Guid.NewGuid(), Name = "Updated Priority" };

            // Act
            await _priorityService.UpdateAsync(priorityDTO.Id, priorityDTO);

            // Assert
            _priorityRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Priority>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeletePriority()
        {
            // Arrange
            var priorityId = Guid.NewGuid();
            _priorityRepositoryMock.Setup(repo => repo.DeleteAsync(priorityId)).ReturnsAsync(true);

            // Act
            var result = await _priorityService.DeleteAsync(priorityId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetTaskCardsByPriorityIdAsync_ShouldReturnTaskCards_WhenTaskCardsExist()
        {
            // Arrange
            var priorityId = Guid.NewGuid();
            var taskCards = new List<TaskCard>
            {
                new TaskCard { Id = Guid.NewGuid(), Title = "TaskCard 1", PriorityId = priorityId },
                new TaskCard { Id = Guid.NewGuid(), Title = "TaskCard 2", PriorityId = priorityId }
            };
            var priority = new Priority { Id = priorityId, Name = "High", TaskCards = taskCards };
            _priorityRepositoryMock.Setup(repo => repo.GetByIdAsync(priorityId)).ReturnsAsync(priority);

            // Act
            var result = await _priorityService.GetTaskCardsByPriorityIdAsync(priorityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}