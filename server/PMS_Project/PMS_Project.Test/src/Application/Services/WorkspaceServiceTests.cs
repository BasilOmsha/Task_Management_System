// using Moq;
// using Xunit;
// using PMS_Project.Application.Abstractions.Services;
// using PMS_Project.Domain.Models;
// using PMS_Project.Domain.Abstractions.Repositories;
// using PMS_Project.Application.DTOs;
// using PMS_Project.Application.Services;
// using AutoMapper;

// namespace PMS_Project.Test.Application.Services
// {
//     public class WorkspaceServiceTests
//     {
//         private readonly Mock<IWorkspaceRepository> _workspaceRepositoryMock;
//         private readonly Mock<IProjectBoardRepository> _projectBoardRepositoryMock;
//         private readonly IWorkspaceService _workspaceService;
//         private readonly IProjectBoardService _projectBoardService;
//         private readonly Mock<IMapper> _mockMapper;
//         private readonly Mock<IWorkspaceUserService> _workspaceUserServiceMock;

//         public WorkspaceServiceTests()
//         {
//             _workspaceRepositoryMock = new Mock<IWorkspaceRepository>();
//             _projectBoardRepositoryMock = new Mock<IProjectBoardRepository>();
//             var roleRepositoryMock = new Mock<IRoleRepository>();
//             _workspaceUserServiceMock = new Mock<IWorkspaceUserService>();
//             _workspaceService = new WorkspaceService(_workspaceRepositoryMock.Object, roleRepositoryMock.Object, _workspaceUserServiceMock.Object);
//             //_workspaceService = new WorkspaceService(_workspaceRepositoryMock.Object, _projectBoardService);
//             var workspaceUserRepoMock = new Mock<IWorkspaceUserRepository>();
//             var roleRepoMock = new Mock<IRoleRepository>();
//             _mockMapper = new Mock<IMapper>();
//             _projectBoardService = new ProjectBoardService(_projectBoardRepositoryMock.Object, _workspaceService, _mockMapper.Object, workspaceUserRepoMock.Object, roleRepoMock.Object);
//             //_mockMapper = new Mock<IMapper>();
//         }

//         [Fact]
//         public async Task GetByIdAsync_ShouldReturnWorkspace_WhenWorkspaceExists()
//         {
//             // Arrange
//             var workspaceId = Guid.NewGuid();
//             var workspace = new Workspace { Id = workspaceId, Name = "Test Workspace", User = new User(), CreatorUserId = workspaceId };
//             _workspaceRepositoryMock.Setup(repo => repo.GetByIdAsync(workspaceId)).ReturnsAsync(workspace);

//             // Act
//             var result = await _workspaceService.GetByIdAsync(workspaceId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(workspaceId, result.Id);
//         }

//         [Fact]
//         public async Task GetAllAsync_ShouldReturnAllWorkspaces()
//         {
//             // Arrange
//             var workspaces = new List<Workspace>
//             {
//                 new Workspace { Id = Guid.NewGuid(), Name = "Workspace 1", User = new User(), CreatorUserId = Guid.NewGuid() },
//                 new Workspace { Id = Guid.NewGuid(), Name = "Workspace 2", User = new User(), CreatorUserId = Guid.NewGuid() }
//             };
//             _workspaceRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(workspaces);

//             // Act
//             var result = await _workspaceService.GetAllAsync();

//             // Assert
//             Assert.Equal(2, result.Count());
//         }

//         [Fact]
//         public async Task AddAsync_ShouldAddWorkspace()
//         {
//             // Arrange
//             var workspaceDto = new CreateWorkspaceDTO { Name = "New Workspace", Description = "Description", IsPublic = true };

//             // Act
//             await _workspaceService.AddAsync(workspaceDto);

//             // Assert
//             _workspaceRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Workspace>(w => w.Name == workspaceDto.Name)), Times.Once);
//         }

//         // [Fact]
//         // public async Task UpdateAsync_ShouldUpdateWorkspace()
//         // {
//         //     // Arrange
//         //     var workspaceDto = new UpdateWorkspaceDTO { Name = "Updated Workspace", Description = "Updated Description", IsPublic = false, UpdatedAt = DateTime.UtcNow};

//         //     // Act
//         //     await _workspaceService.UpdateAsync(workspaceDto.Id, workspaceDto);

//         //     // Assert
//         //     _workspaceRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Workspace>(w => w.Id == workspaceDto.Id)), Times.Once);
//         // }

//         [Fact]
//         public async Task DeleteAsync_ShouldDeleteWorkspace()
//         {
//             // Arrange
//             var workspaceId = Guid.NewGuid();

//             // Act
//             await _workspaceService.DeleteAsync(workspaceId);

//             // Assert
//             _workspaceRepositoryMock.Verify(repo => repo.DeleteAsync(workspaceId), Times.Once);
//         }

//         /*[Fact]
//         public async Task DeleteAsync_ShouldRemoveProjectBoardFromWorkspace()
//         {
//             // Arrange
//             var workspaceId = Guid.NewGuid();
//             var projectBoardId = Guid.NewGuid();
//             var projectBoard = new ProjectBoard { Id = projectBoardId, WorkspaceId = workspaceId };
//             var workspace = new Workspace { Id = workspaceId, ProjectBoards = new List<ProjectBoard> { projectBoard } };

//             _workspaceRepositoryMock.Setup(repo => repo.GetByIdAsync(workspaceId)).ReturnsAsync(workspace);
//             _projectBoardRepositoryMock.Setup(repo => repo.GetByIdAsync(projectBoardId)).ReturnsAsync(projectBoard);

//             // Act
//             await _projectBoardService.DeleteAsync(projectBoardId);

//             // Assert
//             Assert.DoesNotContain(projectBoard, workspace.ProjectBoards);
//             _workspaceRepositoryMock.Verify(repo => repo.UpdateAsync(workspace), Times.Once);
//         }*/

//         [Fact]
//         public async Task AddProjectBoardAsync_ShouldAddProjectBoardToWorkspace()
//         {
//             // Arrange
//             var workspaceId = Guid.NewGuid();
//             var projectBoard = new ProjectBoard { Id = Guid.NewGuid(), WorkspaceId = workspaceId };
//             var workspace = new Workspace { Id = workspaceId, ProjectBoards = new List<ProjectBoard>() };

//             _workspaceRepositoryMock.Setup(repo => repo.GetByIdAsync(workspaceId)).ReturnsAsync(workspace);

//             // Act
//             await _workspaceService.AddProjectBoardAsync(workspaceId, projectBoard);

//             // Assert
//             Assert.Contains(projectBoard, workspace.ProjectBoards);
//             _workspaceRepositoryMock.Verify(repo => repo.UpdateAsync(workspace), Times.Once);
//         }

//         [Fact]
//         public async Task RemoveProjectBoardAsync_ShouldRemoveProjectBoardFromWorkspace()
//         {
//             // Arrange
//             var workspaceId = Guid.NewGuid();
//             var projectBoard = new ProjectBoard { Id = Guid.NewGuid(), WorkspaceId = workspaceId };
//             var workspace = new Workspace { Id = workspaceId, ProjectBoards = new List<ProjectBoard> { projectBoard } };

//             _workspaceRepositoryMock.Setup(repo => repo.GetByIdAsync(workspaceId)).ReturnsAsync(workspace);

//             // Act
//             await _workspaceService.RemoveProjectBoardAsync(workspaceId, projectBoard);

//             // Assert
//             Assert.DoesNotContain(projectBoard, workspace.ProjectBoards);
//             _workspaceRepositoryMock.Verify(repo => repo.UpdateAsync(workspace), Times.Once);
//         }
//     }
// }