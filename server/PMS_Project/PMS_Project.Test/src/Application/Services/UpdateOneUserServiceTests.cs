using Moq;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Domain.Models;
using PMS_Project.Domain.Abstractions.Repositories;
using AutoMapper;
using PMS_Project.Application.Services;

namespace PMS_Project.Test.Application.Services
{
    public class UpdateOneUserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPaswdService> _passwordServiceMock;
        private readonly IUserService _userService;

        public UpdateOneUserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordServiceMock = new Mock<IPaswdService>();
            _userService = new UserService(_userRepoMock.Object, _mapperMock.Object, _passwordServiceMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenValidDataProvided()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Firstname = "OldFirstname",
                Lastname = "OldLastname",
                Username = "oldusername",
                Email = "old@example.com",
                Password = "hashedoldpassword",
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var updateDTO = new UpdateUserInfoDTO
            {
                FirstName = "NewFirstname",
                LastName = "NewLastname",
                Username = "newusername",
                Email = "new@example.com",
                CurrentPassword = "oldpassword",
                NewPassword = "newpassword",
                ConfirmNewPassword = "newpassword"
            };

            var updatedUser = new User
            {
                Id = userId,
                Firstname = "NewFirstname",
                Lastname = "NewLastname",
                Username = "newusername",
                Email = "new@example.com",
                Password = "hashednewpassword",
                UpdatedAt = DateTime.UtcNow
            };

            var expectedDto = new GetUserInfoDTO
            {
                Id = userId,
                FirstName = "NewFirstname",
                LastName = "NewLastname",
                Username = "newusername",
                Email = "new@example.com",
                CreatedAt = existingUser.CreatedAt,
                UpdatedAt = updatedUser.UpdatedAt
            };

            // Mock repository and service behavior
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepoMock.Setup(r => r.GetUserByUserNameAsync(updateDTO.Username)).ReturnsAsync(default(User));
            _userRepoMock.Setup(r => r.GetUserByEmailAsync(updateDTO.Email)).ReturnsAsync(default(User));

            // Mock password service behavior
            _passwordServiceMock.Setup(p => p.VerifyPaswd(updateDTO.CurrentPassword, existingUser.Password))
                .Returns(true); // Validate current password
            _passwordServiceMock.Setup(p => p.HashPaswd(updateDTO.NewPassword))
                .Returns("hashednewpassword"); // Hash new password
            _passwordServiceMock.Setup(p => p.VerifyPaswd(updateDTO.ConfirmNewPassword, "hashednewpassword"))
                .Returns(true); // Validate confirm password

            _userRepoMock.Setup(r => r.UpdateAsync(existingUser)).ReturnsAsync(updatedUser);
            _mapperMock.Setup(m => m.Map<GetUserInfoDTO>(updatedUser)).Returns(expectedDto);

            // Act
            var result = await _userService.UpdateAsync(userId, updateDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.FirstName, result.FirstName);
            Assert.Equal(expectedDto.LastName, result.LastName);
            Assert.Equal(expectedDto.Username, result.Username);
            Assert.Equal(expectedDto.Email, result.Email);

            // Verify the order of operations
            _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
            _passwordServiceMock.Verify(p => p.VerifyPaswd(updateDTO.CurrentPassword, "hashedoldpassword"), Times.Once);
            _passwordServiceMock.Verify(p => p.HashPaswd(updateDTO.NewPassword), Times.Once);
            _passwordServiceMock.Verify(p => p.VerifyPaswd(updateDTO.ConfirmNewPassword, "hashednewpassword"), Times.Once);
            _userRepoMock.Verify(r => r.UpdateAsync(existingUser), Times.Once);
            _mapperMock.Verify(m => m.Map<GetUserInfoDTO>(updatedUser), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenCurrentPasswordIsIncorrect()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Password = "hashedoldpassword",
            };

            var updateDTO = new UpdateUserInfoDTO
            {
                CurrentPassword = "wrong", // Incorrect current password
                // NewPassword = "newpassword",
                // ConfirmNewPassword = "newpassword"
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Mock current password validation to pass when CurrentPassword is different from the existing password and fail otherwise
            _passwordServiceMock.Setup(p => p.VerifyPaswd(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string providedPassword, string storedPassword) =>
                    providedPassword == "oldpassword" && storedPassword == "hashedoldpassword");

            // make sure new password and confirm password match
            _passwordServiceMock.Setup(p => p.VerifyPaswd(updateDTO.ConfirmNewPassword, It.IsAny<string>()))
                .Returns(true);

            // Mock new password hashing
            _passwordServiceMock.Setup(p => p.HashPaswd(It.IsAny<string>()))
                .Returns((string password) => $"hashed-{password}");
                

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateAsync(userId, updateDTO));
            Assert.Equal("Current password is incorrect.", exception.Message);

            // Verify no further processing occurs
            _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once); // Ensure user retrieval is attempted
            _passwordServiceMock.Verify(p => p.VerifyPaswd(updateDTO.CurrentPassword, existingUser.Password), Times.Once); // Ensure current password check
            _passwordServiceMock.Verify(p => p.HashPaswd(It.IsAny<string>()), Times.Never); // Ensure new password is not hashed
            _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never); // Ensure no update is attempted
        }


        //Test should throw exception when password and confirm password do not match
        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenConfirmPasswordDoesNotMatchNewPassword()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Password = "hashedoldpassword" // Simulate the existing hashed password
            };

            var updateDTO = new UpdateUserInfoDTO
            {
                CurrentPassword = "oldpassword", // Correct current password
                NewPassword = "newpassword",    // New password
                ConfirmNewPassword = "wrongpassword" // Intentional mismatch
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Mock current password validation to always pass
            _passwordServiceMock.Setup(p => p.VerifyPaswd(updateDTO.CurrentPassword, existingUser.Password))
                .Returns(true);

            // Mock confirm password validation to fail dynamically
            _passwordServiceMock.Setup(p => p.VerifyPaswd(updateDTO.ConfirmNewPassword, It.IsAny<string>()))
                .Returns((string confirmPassword, string hashedPassword) =>
                    confirmPassword == updateDTO.NewPassword);

            // Mock new password hashing
            _passwordServiceMock.Setup(p => p.HashPaswd(It.IsAny<string>()))
                .Returns((string password) => $"hashed-{password}");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateAsync(userId, updateDTO));
            Assert.Equal("New password and confirm password do not match.", exception.Message);

            // Verify interactions
            _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
            _passwordServiceMock.Verify(p => p.VerifyPaswd(updateDTO.CurrentPassword, existingUser.Password), Times.Once);
            _passwordServiceMock.Verify(p => p.HashPaswd(updateDTO.NewPassword), Times.Once);
            _passwordServiceMock.Verify(p => p.VerifyPaswd(updateDTO.ConfirmNewPassword, $"hashed-{updateDTO.NewPassword}"), Times.Once);
            _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

    }
}
