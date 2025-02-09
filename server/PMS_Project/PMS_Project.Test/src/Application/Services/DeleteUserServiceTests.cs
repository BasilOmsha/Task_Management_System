using AutoMapper;
using Moq;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.Services;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Test.Application.Services
{
    public class DeleteUserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPaswdService> _passwordServiceMock;
        private readonly IUserService _userService;

        public DeleteUserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordServiceMock = new Mock<IPaswdService>();
            _userService = new UserService(_userRepoMock.Object, _mapperMock.Object, _passwordServiceMock.Object);
        }
        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null); // Simulate user not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteAsync(userId));
            Assert.Equal("User not found.", exception.Message);

            // Verify no delete operation is called
            _userRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user); // Simulate user found
            _userRepoMock.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(true); // Simulate successful delete

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.True(result);

            // Verify both retrieval and deletion
            _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteAsync(userId), Times.Once);
        }

    }
}