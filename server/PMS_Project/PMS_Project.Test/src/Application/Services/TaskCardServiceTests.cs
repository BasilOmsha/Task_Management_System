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
//     public class TaskCardServiceTests
//     {
//         private readonly Mock<ITaskCardRepository> _taskCardRepositoryMock;
//         private readonly ITaskCardService _taskCardService;
//         private readonly Mock<IUserRepository> _userRepoMock;
//         private readonly IUserService _userService;

//         public TaskCardServiceTests()
//         {
//             _taskCardRepositoryMock = new Mock<ITaskCardRepository>();
//             _userRepoMock = new Mock<IUserRepository>();
//             var mapperMock = new Mock<IMapper>();
//             _taskCardService = new TaskCardService(_taskCardRepositoryMock.Object, mapperMock.Object, _userRepoMock.Object);
//             var paswdServiceMock = new Mock<IPaswdService>();
//             _userService = new UserService(_userRepoMock.Object, mapperMock.Object, paswdServiceMock.Object);
//         }

//         [Fact]
//         public async Task GetByIdAsync_ShouldReturnTaskCard_WhenTaskCardExists()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var taskCard = new TaskCard { Id = taskCardId, Title = "Test TaskCard" };
//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);

//             // Act
//             var result = await _taskCardService.GetByIdAsync(taskCardId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(taskCardId, result.Id);
//         }

//         /*[Fact]
//         public async Task GetByIdAsync_ShouldReturnTaskCardWithActivities_WhenTaskCardExists()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var activities = new List<TaskCardActivity>
//             {
//                 new TaskCardActivity { Id = Guid.NewGuid(), TaskCardId = taskCardId, Activity = "Activity 1" },
//                 new TaskCardActivity { Id = Guid.NewGuid(), TaskCardId = taskCardId, Activity = "Activity 2" }
//             };
//             var taskCard = new TaskCard { Id = taskCardId, Title = "Test TaskCard", Activities = activities };
//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);

//             // Act
//             var result = await _taskCardService.GetByIdAsync(taskCardId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(taskCardId, result.Id);
//             Assert.Equal(2, result.Activities.Count);
//         }*/

//         [Fact]
//         public async Task GetAllAsync_ShouldReturnAllTaskCards()
//         {
//             // Arrange
//             var taskCards = new List<TaskCard>
//             {
//                 new TaskCard { Id = Guid.NewGuid(), Title = "TaskCard 1" },
//                 new TaskCard { Id = Guid.NewGuid(), Title = "TaskCard 2" }
//             };
//             _taskCardRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(taskCards);

//             // Act
//             var result = await _taskCardService.GetAllAsync();

//             // Assert
//             Assert.Equal(2, result.Count());
//         }

//         /*[Fact]
//         public async Task GetAllAsync_ShouldReturnTaskCardsWithActivities()
//         {
//             // Arrange
//             var taskCardId1 = Guid.NewGuid();
//             var taskCardId2 = Guid.NewGuid();
//             var taskCards = new List<TaskCard>
//             {
//                 new TaskCard { Id = taskCardId1, Title = "TaskCard 1", Activities = new List<TaskCardActivity>
//                 {
//                     new TaskCardActivity { Id = Guid.NewGuid(), TaskCardId = taskCardId1, Activity = "Activity 1" }
//                 }},
//                 new TaskCard { Id = taskCardId2, Title = "TaskCard 2", Activities = new List<TaskCardActivity>
//                 {
//                     new TaskCardActivity { Id = Guid.NewGuid(), TaskCardId = taskCardId2, Activity = "Activity 2" }
//                 }}
//             };
//             _taskCardRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(taskCards);

//             // Act
//             var result = await _taskCardService.GetAllAsync();

//             // Assert
//             Assert.Equal(2, result.Count());
//             Assert.Single(result.First().Activities);
//             Assert.Single(result.Last().Activities);
//         }*/

//         [Fact]
//         public async Task AddAsync_ShouldAddTaskCard()
//         {
//             // Arrange
//             var taskCardDTO = new TaskCardDTO { Id = Guid.NewGuid(), Title = "New TaskCard" };

//             // Act
//             await _taskCardService.AddAsync(taskCardDTO);

//             // Assert
//             _taskCardRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskCard>()), Times.Once);
//         }

//         [Fact]
//         public async Task UpdateAsync_ShouldUpdateTaskCard()
//         {
//             // Arrange
//             var taskCardDTO = new TaskCardDTO { Id = Guid.NewGuid(), Title = "Updated TaskCard" };

//             // Act
//             await _taskCardService.UpdateAsync(taskCardDTO.Id, taskCardDTO);

//             // Assert
//             _taskCardRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskCard>()), Times.Once);
//         }

//         [Fact]
//         public async Task UpdateAsync_ShouldUpdateTaskCardWithStatus()
//         {
//             // Arrange
//             var status = new StatusDTO { Id = Guid.NewGuid(), Name = "In Progress" };
//             var taskCardDTO = new TaskCardDTO { Id = Guid.NewGuid(), Title = "Updated TaskCard", Status = status };

//             // Act
//             await _taskCardService.UpdateAsync(taskCardDTO.Id, taskCardDTO);

//             // Assert
//             _taskCardRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<TaskCard>(tc => tc.Status != null && tc.Status.Id == status.Id && tc.Status.Name == status.Name)), Times.Once);
//         }

//         [Fact]
//         public async Task DeleteAsync_ShouldDeleteTaskCard()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var taskCard = new TaskCard { Id = taskCardId, Title = "Test TaskCard" };
//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);

//             // Act
//             await _taskCardService.DeleteAsync(taskCardId);

//             // Assert
//             _taskCardRepositoryMock.Verify(repo => repo.DeleteAsync(taskCardId), Times.Once);
//         }

//         [Fact]
//         public async Task AddTaskCardActivityAsync_ShouldAddActivityToTaskCard()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var activity = new TaskCardActivity { Id = Guid.NewGuid(), TaskCardId = taskCardId, Activity = "New Activity" };
//             //var activityDTO = new TaskCardActivityDTO { Id = activity.Id, TaskCardId = activity.TaskCardId, Activity = activity.Activity };
//             var taskCard = new TaskCard { Id = taskCardId, Activities = new List<TaskCardActivity>() };

//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);

//             //await _taskCardService.AddTaskCardActivityAsync(taskCardId, activityDTO);
//             await _taskCardService.AddTaskCardActivityAsync(taskCardId, activity);

//             // Assert
//             Assert.Contains(activity, taskCard.Activities);
//             _taskCardRepositoryMock.Verify(repo => repo.UpdateAsync(taskCard), Times.Once);
//         }

//         [Fact]
//         public async Task RemoveTaskCardActivityAsync_ShouldRemoveActivityFromTaskCard()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var activity = new TaskCardActivity { Id = Guid.NewGuid(), TaskCardId = taskCardId, Activity = "Activity to Remove" };
//             var taskCard = new TaskCard { Id = taskCardId, Activities = new List<TaskCardActivity> { activity } };

//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);

//             // Act
//             await _taskCardService.RemoveTaskCardActivityAsync(taskCardId, activity);

//             // Assert
//             Assert.DoesNotContain(activity, taskCard.Activities);
//             _taskCardRepositoryMock.Verify(repo => repo.UpdateAsync(taskCard), Times.Once);
//         }

//         [Fact]
//         public async Task AddTaskCardActivityAsync_ShouldAddActivityToUser()
//         {
//             // Arrange
//             var userId = Guid.NewGuid();
//             var activity = new TaskCardActivity { Id = Guid.NewGuid(), UserId = userId, Activity = "New Activity" };
//             var user = new User { Id = userId, TaskCardActivities = new List<TaskCardActivity>() };

//             _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

//             // Act
//             await _userService.AddTaskCardActivityAsync(userId, activity);

//             // Assert
//             Assert.Contains(activity, user.TaskCardActivities);
//             _userRepoMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
//         }

//         [Fact]
//         public async Task RemoveTaskCardActivityAsync_ShouldRemoveActivityFromUser()
//         {
//             // Arrange
//             var userId = Guid.NewGuid();
//             var activity = new TaskCardActivity { Id = Guid.NewGuid(), UserId = userId, Activity = "Activity to Remove" };
//             var user = new User { Id = userId, TaskCardActivities = new List<TaskCardActivity> { activity } };

//             _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

//             // Act
//             await _userService.RemoveTaskCardActivityAsync(userId, activity);

//             // Assert
//             Assert.DoesNotContain(activity, user.TaskCardActivities);
//             _userRepoMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
//         }

//         [Fact]
//         public async Task AddUserToTaskCardAsync_ShouldAddUserToTaskCard()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var userId = Guid.NewGuid();
//             var taskCard = new TaskCard { Id = taskCardId, TaskCard_Users = new List<TaskCard_User>() };
//             var user = new User { Id = userId };

//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);
//             _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

//             // Act
//             await _taskCardService.AddUserToTaskCardAsync(taskCardId, userId);

//             // Assert
//             Assert.Contains(taskCard.TaskCard_Users, tcu => tcu.TaskCardId == taskCardId && tcu.UserId == userId);
//             _taskCardRepositoryMock.Verify(repo => repo.UpdateAsync(taskCard), Times.Once);
//         }

//         [Fact]
//         public async Task RemoveUserFromTaskCardAsync_ShouldRemoveUserFromTaskCard()
//         {
//             // Arrange
//             var taskCardId = Guid.NewGuid();
//             var userId = Guid.NewGuid();
//             var taskCardUser = new TaskCard_User { TaskCardId = taskCardId, UserId = userId };
//             var taskCard = new TaskCard { Id = taskCardId, TaskCard_Users = new List<TaskCard_User> { taskCardUser } };
//             var user = new User { Id = userId };

//             _taskCardRepositoryMock.Setup(repo => repo.GetByIdAsync(taskCardId)).ReturnsAsync(taskCard);
//             _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

//             // Act
//             await _taskCardService.RemoveUserFromTaskCardAsync(taskCardId, userId);

//             // Assert
//             Assert.DoesNotContain(taskCard.TaskCard_Users, tcu => tcu.TaskCardId == taskCardId && tcu.UserId == userId);
//             _taskCardRepositoryMock.Verify(repo => repo.UpdateAsync(taskCard), Times.Once);
//         }
//     }
// }