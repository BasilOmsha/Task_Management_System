using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Services;
using PMS_Project.Domain.Models;
using AutoMapper;
using Moq;
using Xunit;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.Services;

namespace PMS_Project.Test.Application.Services
{
    public class TaskCardActivityServiceTests
    {
        private readonly Mock<ITaskCardActivityRepository> _taskCardActivityRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ITaskCardActivityService _taskCardActivityService;

        public TaskCardActivityServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskCardActivity, TaskCardActivityDTO>();
                cfg.CreateMap<TaskCardActivityDTO, TaskCardActivity>();
            });
            _mapper = config.CreateMapper();
            _taskCardActivityRepositoryMock = new Mock<ITaskCardActivityRepository>();
            _taskCardActivityService = new TaskCardActivityService(_taskCardActivityRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTaskCardActivity()
        {
            // Arrange
            var activityDTO = new TaskCardActivityDTO { Id = Guid.NewGuid(), Activity = "New Activity" };

            // Act
            await _taskCardActivityService.AddAsync(activityDTO);

            // Assert
            _taskCardActivityRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskCardActivity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTaskCardActivity()
        {
            // Arrange
            var activityDTO = new TaskCardActivityDTO { Id = Guid.NewGuid(), Activity = "Updated Activity" };

            // Act
            await _taskCardActivityService.UpdateAsync(activityDTO.Id, activityDTO);

            // Assert
            _taskCardActivityRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskCardActivity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTaskCardActivity()
        {
            // Arrange
            var activityId = Guid.NewGuid();

            // Act
            await _taskCardActivityService.DeleteAsync(activityId);

            // Assert
            _taskCardActivityRepositoryMock.Verify(repo => repo.DeleteAsync(activityId), Times.Once);
        }

        [Fact]
        public async Task GetByTaskCardIdAsync_ShouldReturnActivities()
        {
            // Arrange
            var taskCardId = Guid.NewGuid();

            // Act
            var activities = await _taskCardActivityService.GetByTaskCardIdAsync(taskCardId);

            // Assert
            _taskCardActivityRepositoryMock.Verify(repo => repo.GetByTaskCardIdAsync(taskCardId), Times.Once);
        }
    }
}