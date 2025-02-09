using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Application.Services;
using PMS_Project.Domain.Models;
using AutoMapper;
using Moq;
using Xunit;
using PMS_Project.Application.Abstractions.Services;

namespace PMS_Project.Test.Application.Services
{
    public class StatusServiceTests
    {
        private readonly Mock<IStatusRepository> _statusRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IStatusService _statusService;

        public StatusServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<StatusDTO, Status>();
                cfg.CreateMap<TaskCard, TaskCardDTO>();
            });
            _mapper = config.CreateMapper();
            _statusRepositoryMock = new Mock<IStatusRepository>();
            _statusService = new StatusService(_statusRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStatuses()
        {
            // Arrange
            var statuses = new List<Status>
            {
                new Status { Id = Guid.NewGuid(), Name = "Status 1" },
                new Status { Id = Guid.NewGuid(), Name = "Status 2" }
            };
            _statusRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(statuses);

            // Act
            var result = await _statusService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_ShouldAddStatus()
        {
            // Arrange
            var statusDTO = new StatusDTO { Name = "New Status" };
            var status = new Status { Id = Guid.NewGuid(), Name = "New Status" };
            _statusRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Status>())).ReturnsAsync(status);

            // Act
            var result = await _statusService.AddAsync(statusDTO);

            // Assert
            Assert.Equal(statusDTO.Name, result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStatus()
        {
            // Arrange
            var statusDTO = new StatusDTO { Id = Guid.NewGuid(), Name = "Updated Status" };

            // Act
            await _statusService.UpdateAsync(statusDTO.Id, statusDTO);

            // Assert
            _statusRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Status>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteStatus()
        {
            // Arrange
            var statusId = Guid.NewGuid();
            _statusRepositoryMock.Setup(repo => repo.DeleteAsync(statusId)).ReturnsAsync(true);

            // Act
            var result = await _statusService.DeleteAsync(statusId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetTaskCardsByStatusIdAsync_ShouldReturnTaskCards_WhenTaskCardsExist()
        {
            // Arrange
            var statusId = Guid.NewGuid();
            var taskCards = new List<TaskCard>
            {
                new TaskCard { Id = Guid.NewGuid(), Title = "TaskCard 1", StatusId = statusId },
                new TaskCard { Id = Guid.NewGuid(), Title = "TaskCard 2", StatusId = statusId }
            };
            var status = new Status { Id = statusId, Name = "Status 1", TaskCards = taskCards };
            _statusRepositoryMock.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(status);

            // Act
            var result = await _statusService.GetTaskCardsByStatusIdAsync(statusId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}