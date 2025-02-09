// using Moq;
// using Xunit;
// using PMS_Project.Application.Abstractions.Services;
// using PMS_Project.Domain.Models;
// using PMS_Project.Infrastructure.Repositories;
// using PMS_Project.Domain.Abstractions.Repositories;
// using PMS_Project.Application.DTOs;
// using PMS_Project.Application.Services;
// using AutoMapper;

// namespace PMS_Project.Test.Application.Services
// {
//     public class ProjectBoardServiceTests
//     {
//         private readonly Mock<IProjectBoardRepository> _projectBoardRepositoryMock;
//         private readonly Mock<IWorkspaceService> _workspaceServiceMock;
//         private readonly IProjectBoardService _projectBoardService;

//         public ProjectBoardServiceTests()
//         {
//             _workspaceServiceMock = new Mock<IWorkspaceService>();
//             _projectBoardRepositoryMock = new Mock<IProjectBoardRepository>();
//             var config = new MapperConfiguration(cfg =>
//             {
//                 cfg.CreateMap<ProjectBoard, ProjectBoardDTO>();
//                 cfg.CreateMap<ProjectBoardDTO, ProjectBoard>();
//                 cfg.CreateMap<CreateProjectBoardDTO, ProjectBoard>();
//                 cfg.CreateMap<UpdateProjectBoardDTO, ProjectBoard>();
//             });
//             var mapper = config.CreateMapper();
//             var workspaceUserRepoMock = new Mock<IWorkspaceUserRepository>();
//             var roleRepoMock = new Mock<IRoleRepository>();
//             _projectBoardService = new ProjectBoardService(_projectBoardRepositoryMock.Object, _workspaceServiceMock.Object, mapper, workspaceUserRepoMock.Object, roleRepoMock.Object);
//         }

//         [Fact]
//         public async Task GetByIdAsync_ShouldReturnProjectBoard_WhenProjectBoardExists()
//         {
//             // Arrange
//             var projectBoardId = Guid.NewGuid();
//             var projectBoard = new ProjectBoard { Id = projectBoardId, Name = "Test ProjectBoard" };
//             _projectBoardRepositoryMock.Setup(repo => repo.GetByIdAsync(projectBoardId)).ReturnsAsync(projectBoard);

//             // Act
//             var result = await _projectBoardService.GetByIdAsync(projectBoardId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(projectBoardId, result.Id);
//         }

//         [Fact]
//         public async Task GetAllAsync_ShouldReturnAllProjectBoards()
//         {
//             // Arrange
//             var projectBoards = new List<ProjectBoard>
//             {
//                 new ProjectBoard { Id = Guid.NewGuid(), Name = "ProjectBoard 1" },
//                 new ProjectBoard { Id = Guid.NewGuid(), Name = "ProjectBoard 2" }
//             };
//             _projectBoardRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projectBoards);

//             // Act
//             var result = await _projectBoardService.GetAllAsync();

//             // Assert
//             Assert.Equal(2, result.Count());
//         }

//         [Fact]
//         public async Task AddAsync_ShouldAddProjectBoard()
//         {
//             // Arrange
//             var createProjectBoardDTO = new CreateProjectBoardDTO { Name = "New ProjectBoard", Description = "Description is required" };

//             // Act
//             await _projectBoardService.AddAsync(createProjectBoardDTO);

//             // Assert
//             _projectBoardRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ProjectBoard>()), Times.Once);
//         }

//         [Fact]
//         public async Task UpdateAsync_ShouldUpdateProjectBoard()
//         {
//             // Arrange
//             var projectBoardDTO = new ProjectBoardDTO { Id = Guid.NewGuid(), Name = "Updated ProjectBoard" };
//             var updateProjectBoardDTO = new UpdateProjectBoardDTO { Name = projectBoardDTO.Name };

//             // Act
//             await _projectBoardService.UpdateAsync(projectBoardDTO.Id, updateProjectBoardDTO);

//             // Assert
//             _projectBoardRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ProjectBoard>()), Times.Once);
//         }

//         [Fact]
//         public async Task DeleteAsync_ShouldDeleteProjectBoard()
//         {
//             // Arrange
//             var projectBoardId = Guid.NewGuid();
//             var projectBoard = new ProjectBoard { Id = projectBoardId, Name = "Test ProjectBoard" };
//             _projectBoardRepositoryMock.Setup(repo => repo.GetByIdAsync(projectBoardId)).ReturnsAsync(projectBoard);

//             // Act
//             await _projectBoardService.DeleteAsync(projectBoardId);

//             // Assert
//             _projectBoardRepositoryMock.Verify(repo => repo.DeleteAsync(projectBoardId), Times.Once);
//         }
//     }
// }