using AutoMapper;
using Moq;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Application.Services;

namespace PMS_Project.Test.Application.Services
{
    public class CreateUserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPaswdService> _passwordServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserService _userService;

        public CreateUserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPaswdService>();
            _mapperMock = new Mock<IMapper>();

            _userService = new UserService(
                _userRepoMock.Object,
                _mapperMock.Object,
                _passwordServiceMock.Object
            );
        }

        //Test should create user when valid data is provided
        [Fact]
        public async Task AddAsync_ShouldCreateUser_WhenValidDataProvided()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Firstname = "John",
                Lastname = "Doe",
                Username = "johndoe",
                Email = "john.doe@example.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Firstname = createUserDto.Firstname,
                Lastname = createUserDto.Lastname,
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Password = "hashedpassword",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var getUserInfoDto = new GetUserInfoDTO
            {
                Id = user.Id,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            // Setup mocks
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<CreateUserDTO>())).Returns(user);
            _userRepoMock.Setup(r => r.GetUserByEmailAsync(createUserDto.Email)).ReturnsAsync(default(User));
            _userRepoMock.Setup(r => r.GetUserByUserNameAsync(createUserDto.Username)).ReturnsAsync(default(User));
            _passwordServiceMock.Setup(p => p.HashPaswd(createUserDto.Password)).Returns("hashedpassword");
            _passwordServiceMock.Setup(p => p.VerifyPaswd(createUserDto.ConfirmPassword, "hashedpassword")).Returns(true);

            // Mock repository behavior. whuci will return the user object that was passed in
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Mock mapper behavior. which will return the getUserInfoDto object that was passed in
            _mapperMock.Setup(m => m.Map<GetUserInfoDTO>(It.IsAny<User>())).Returns(getUserInfoDto);

            // Act
            var result = await _userService.AddAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(getUserInfoDto.Id, result.Id);
            Assert.Equal(getUserInfoDto.Email, result.Email);
            Assert.Equal(getUserInfoDto.FirstName, result.FirstName);
        }

        //Test should throw exception when email already exists
        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Email = "alice.smith@example.com",
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "alice.smith@example.com",
            };

            // Mock property mapping
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<CreateUserDTO>())).Returns((CreateUserDTO dto) => new User
            {
                Email = dto.Email,
    
            });

            // Mock repository behavior to dynamically check for email conflict
            _userRepoMock.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => email == existingUser.Email ? existingUser : null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.AddAsync(createUserDto));
            Assert.Equal("Email already exists", exception.Message);

        }

        //Test should throw exception when username already exists
        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Username = "alicesmith",
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "alicesmith",
            };

            // Mock property mapping
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<CreateUserDTO>())).Returns((CreateUserDTO dto) => new User
            {
                Username = dto.Username,
            });


            // Mock repository behavior to check for username conflict
            _userRepoMock.Setup(r => r.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => username == existingUser.Username ? existingUser : null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.AddAsync(createUserDto));
            Assert.Equal("Username already exists", exception.Message);
        
        }

        //Test should throw exception when password and confirm password do not match
        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenPasswordAndConfirmPasswordDoNotMatch()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Firstname = "Alice",
                Lastname = "Smith",
                Username = "alicesmith",
                Email = "alice.sm@ex.net",
                Password = "Password@123",
                ConfirmPassword = "Password@1231234234"
            };

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<CreateUserDTO>())).Returns((CreateUserDTO dto) => new User
            {
                Email = dto.Email,
                Username = dto.Username,
                Firstname = dto.Firstname,
                Lastname = dto.Lastname,
                Password = dto.Password
            });

            _userRepoMock.Setup(r => r.GetUserByEmailAsync(createUserDto.Email)).ReturnsAsync((User)null);
            _userRepoMock.Setup(r => r.GetUserByUserNameAsync(createUserDto.Username)).ReturnsAsync((User)null);

            // Simulate hashing the password dynamically
            _passwordServiceMock.Setup(p => p.HashPaswd(It.IsAny<string>()))
                .Returns((string password) => $"hashed-{password}");

            // Simulate dynamic password verification
            _passwordServiceMock.Setup(p => p.VerifyPaswd(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string confirmPassword, string hashedPassword) => hashedPassword == $"hashed-{confirmPassword}");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.AddAsync(createUserDto));
            Assert.Equal("Password and confirm password do not match.", exception.Message);
        }

    }
}