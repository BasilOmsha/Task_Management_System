using Moq;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Domain.Models;
using PMS_Project.Domain.Abstractions.Repositories;
using AutoMapper;
using Xunit;
using PMS_Project.Application.Services;

namespace PMS_Project.Test.Application.Services
{
    public class GetOneUserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserService _userService;

        public GetOneUserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepoMock.Object, _mapperMock.Object, null); // Pass null for unused dependencies
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Firstname = "Alice",
                Lastname = "Smith",
                Username = "alicesmith",
                Email = "alice.smith@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var expectedDto = new GetUserInfoDTO
            {
                Id = userId,
                FirstName = "Alice",
                LastName = "Smith",
                Username = "alicesmith",
                Email = "alice.smith@example.com",
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<GetUserInfoDTO>(user)).Returns(expectedDto);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.FirstName, result.FirstName);
            Assert.Equal(expectedDto.LastName, result.LastName);
            Assert.Equal(expectedDto.Username, result.Username);
            Assert.Equal(expectedDto.Email, result.Email);
            Assert.Equal(expectedDto.CreatedAt, result.CreatedAt);
            Assert.Equal(expectedDto.UpdatedAt, result.UpdatedAt);

            // Verify calls
            _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetUserInfoDTO>(user), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetByIdAsync(userId));

            // Verify calls
            _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetUserInfoDTO>(It.IsAny<User>()), Times.Never);
        }
    }
}
