using Moq;
using Xunit;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Services;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Test.Application.Services
{
    public class ProjectBoardLabelServiceTests
    {
        private readonly Mock<IProjectBoardLabelRepository> _projectBoardLabelRepositoryMock;
        private readonly IProjectBoardLabelService _projectBoardLabelService;

        public ProjectBoardLabelServiceTests()
        {
            _projectBoardLabelRepositoryMock = new Mock<IProjectBoardLabelRepository>();
            _projectBoardLabelService = new ProjectBoardLabelService(_projectBoardLabelRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProjectBoardLabel()
        {
            // Arrange
            var projectBoardLabelDTO = new ProjectBoardLabelDTO { Id = Guid.NewGuid(), ProjectBoardId = Guid.NewGuid(), Name = "Label", Color = "#FFFFFF", TaskCardIds = new List<Guid> { Guid.NewGuid() } };

            // Act
            await _projectBoardLabelService.AddAsync(projectBoardLabelDTO);

            // Assert
            _projectBoardLabelRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ProjectBoardLabel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProjectBoardLabel()
        {
            // Arrange
            var projectBoardLabelDTO = new ProjectBoardLabelDTO { Id = Guid.NewGuid(), ProjectBoardId = Guid.NewGuid(), Name = "Updated Label", Color = "#000000", TaskCardIds = new List<Guid> { Guid.NewGuid() } };

            // Act
            await _projectBoardLabelService.UpdateAsync(projectBoardLabelDTO.Id, projectBoardLabelDTO);

            // Assert
            _projectBoardLabelRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ProjectBoardLabel>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProjectBoardLabel()
        {
            // Arrange
            var projectBoardLabelId = Guid.NewGuid();
            _projectBoardLabelRepositoryMock.Setup(repo => repo.DeleteAsync(projectBoardLabelId)).ReturnsAsync(true);

            // Act
            var result = await _projectBoardLabelService.DeleteAsync(projectBoardLabelId);

            // Assert
            Assert.True(result);
            _projectBoardLabelRepositoryMock.Verify(repo => repo.DeleteAsync(projectBoardLabelId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProjectBoardLabel_WhenProjectBoardLabelExists()
        {
            // Arrange
            var projectBoardLabelId = Guid.NewGuid();
            var projectBoardLabel = new ProjectBoardLabel { Id = projectBoardLabelId, ProjectBoardId = Guid.NewGuid(), Name = "Label", Color = "#FFFFFF" };
            _projectBoardLabelRepositoryMock.Setup(repo => repo.GetByIdAsync(projectBoardLabelId)).ReturnsAsync(projectBoardLabel);

            // Act
            var result = await _projectBoardLabelService.GetByIdAsync(projectBoardLabelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectBoardLabelId, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProjectBoardLabels()
        {
            // Arrange
            var projectBoardLabels = new List<ProjectBoardLabel>
            {
                new ProjectBoardLabel { Id = Guid.NewGuid(), ProjectBoardId = Guid.NewGuid(), Name = "Label 1", Color = "#FFFFFF" },
                new ProjectBoardLabel { Id = Guid.NewGuid(), ProjectBoardId = Guid.NewGuid(), Name = "Label 2", Color = "#000000" }
            };
            _projectBoardLabelRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projectBoardLabels);

            // Act
            var result = await _projectBoardLabelService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}