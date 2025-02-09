// PMS_Project.Tests/Application/Services/RoleServiceTests.cs

using AutoMapper;
using Moq;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Services;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Tests.Application.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _mockRoleRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _mockRoleRepo = new Mock<IRoleRepository>();
            _mockMapper = new Mock<IMapper>();
            _roleService = new RoleService(_mockRoleRepo.Object, _mockMapper.Object);
        }

        // Helper method to create a sample Role entity
        private Role GetSampleRole(Guid? id = null, string name = "Admin", string description = "Administrator role")
        {
            return new Role
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                Description = description
            };
        }

        [Fact]
        public async Task AddAsync_ShouldAddRole_WhenRoleNameIsUnique()
        {
            // Arrange
            var createRoleDto = new CreateRoleDTO
            {
                Name = "Manager",
                Description = "Manager role"
            };

            var roleEntity = GetSampleRole(null, createRoleDto.Name, createRoleDto.Description);
            var getRoleDto = new GetRoleDTO
            {
                Id = roleEntity.Id,
                Name = roleEntity.Name,
                Description = roleEntity.Description
            };

            _mockRoleRepo.Setup(repo => repo.GetRoleByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync((Role)null); // No existing role with the same name

            _mockMapper.Setup(m => m.Map<Role>(It.IsAny<CreateRoleDTO>()))
                       .Returns(roleEntity);

            _mockRoleRepo.Setup(repo => repo.AddAsync(It.IsAny<Role>()))
                        .ReturnsAsync(roleEntity);

            _mockMapper.Setup(m => m.Map<GetRoleDTO>(It.IsAny<Role>()))
                       .Returns(getRoleDto);

            // Act
            var result = await _roleService.AddAsync(createRoleDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(getRoleDto.Id, result.Id);
            Assert.Equal(getRoleDto.Name, result.Name);
            Assert.Equal(getRoleDto.Description, result.Description);

            _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(createRoleDto.Name), Times.Once);
            _mockMapper.Verify(m => m.Map<Role>(createRoleDto), Times.Once);
            _mockRoleRepo.Verify(repo => repo.AddAsync(roleEntity), Times.Once);
            _mockMapper.Verify(m => m.Map<GetRoleDTO>(roleEntity), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowArgumentException_WhenRoleNameAlreadyExists()
        {
            // Arrange
            var createRoleDto = new CreateRoleDTO
            {
                Name = "Admin",
                Description = "Administrator role"
            };

            var existingRole = GetSampleRole(null, createRoleDto.Name, "Existing admin role");

            _mockRoleRepo.Setup(repo => repo.GetRoleByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(existingRole); // Existing role with the same name

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _roleService.AddAsync(createRoleDto));
            Assert.Equal("Role already exists.", exception.Message);

            _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(createRoleDto.Name), Times.Once);
            _mockMapper.Verify(m => m.Map<Role>(It.IsAny<CreateRoleDTO>()), Times.Never);
            _mockRoleRepo.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetRoleDTO>(It.IsAny<Role>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowArgumentNullException_WhenCreateDTOIsNull()
        {
            // Arrange
            CreateRoleDTO createRoleDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roleService.AddAsync(createRoleDto));

            _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(It.IsAny<string>()), Times.Never);
            _mockMapper.Verify(m => m.Map<Role>(It.IsAny<CreateRoleDTO>()), Times.Never);
            _mockRoleRepo.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetRoleDTO>(It.IsAny<Role>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRole_WhenNewRoleNameIsUnique()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var updateRoleDto = new UpdateRoleDTO
            {
                Name = "SuperAdmin",
                Description = "Super Administrator role"
            };

            var existingRole = GetSampleRole(roleId, "Admin", "Administrator role");
            var updatedRole = GetSampleRole(roleId, updateRoleDto.Name, updateRoleDto.Description);
            var getRoleDto = new GetRoleDTO
            {
                Id = updatedRole.Id,
                Name = updatedRole.Name,
                Description = updatedRole.Description
            };

            _mockRoleRepo.Setup(repo => repo.GetByIdAsync(roleId))
                        .ReturnsAsync(existingRole); // Role exists

            _mockRoleRepo.Setup(repo => repo.GetRoleByNameAsync(updateRoleDto.Name))
                        .ReturnsAsync((Role)null); // No role with the new name

            _mockMapper.Setup(m => m.Map(updateRoleDto, existingRole))
                       .Callback<UpdateRoleDTO, Role>((dto, role) =>
                       {
                           role.Name = dto.Name;
                           role.Description = dto.Description;
                       })
                       .Returns(existingRole);

            _mockRoleRepo.Setup(repo => repo.UpdateAsync(existingRole))
                        .ReturnsAsync(updatedRole);

            _mockMapper.Setup(m => m.Map<GetRoleDTO>(updatedRole))
                       .Returns(getRoleDto);

            // Act
            var result = await _roleService.UpdateAsync(roleId, updateRoleDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(getRoleDto.Id, result.Id);
            Assert.Equal(getRoleDto.Name, result.Name);
            Assert.Equal(getRoleDto.Description, result.Description);

            // _mockRoleRepo.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
            // _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(updateRoleDto.Name), Times.Once);
            // _mockMapper.Verify(m => m.Map(updateRoleDto, existingRole), Times.Once);
            // _mockRoleRepo.Verify(repo => repo.UpdateAsync(existingRole), Times.Once);
            // _mockMapper.Verify(m => m.Map<GetRoleDTO>(updatedRole), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenRoleDoesNotExist()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var updateRoleDto = new UpdateRoleDTO
            {
                Name = "SuperAdmin",
                Description = "Super Administrator role"
            };

            _mockRoleRepo.Setup(repo => repo.GetByIdAsync(roleId))
                        .ReturnsAsync((Role)null); // Role does not exist

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.UpdateAsync(roleId, updateRoleDto));
            Assert.Equal("Role does not exist.", exception.Message);

            _mockRoleRepo.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
            _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(It.IsAny<string>()), Times.Never);
            _mockMapper.Verify(m => m.Map(It.IsAny<UpdateRoleDTO>(), It.IsAny<Role>()), Times.Never);
            _mockRoleRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetRoleDTO>(It.IsAny<Role>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenNewRoleNameAlreadyExists()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var updateRoleDto = new UpdateRoleDTO
            {
                Name = "Manager", // Assuming "Manager" already exists
                Description = "Updated Description"
            };

            var existingRole = GetSampleRole(roleId, "Admin", "Administrator role");
            var duplicateRole = GetSampleRole(Guid.NewGuid(), "Manager", "Manager role");

            _mockRoleRepo.Setup(repo => repo.GetByIdAsync(roleId))
                        .ReturnsAsync(existingRole); // Role exists

            _mockRoleRepo.Setup(repo => repo.GetRoleByNameAsync(updateRoleDto.Name))
                        .ReturnsAsync(duplicateRole); // Duplicate role exists

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _roleService.UpdateAsync(roleId, updateRoleDto));
            Assert.Equal("Another role with the same name already exists.", exception.Message);

            _mockRoleRepo.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
            _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(updateRoleDto.Name), Times.Once);
            _mockMapper.Verify(m => m.Map(It.IsAny<UpdateRoleDTO>(), It.IsAny<Role>()), Times.Never);
            _mockRoleRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetRoleDTO>(It.IsAny<Role>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenUpdateDTOIsNull()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            UpdateRoleDTO updateRoleDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roleService.UpdateAsync(roleId, updateRoleDto));

            _mockRoleRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _mockRoleRepo.Verify(repo => repo.GetRoleByNameAsync(It.IsAny<string>()), Times.Never);
            _mockMapper.Verify(m => m.Map(It.IsAny<UpdateRoleDTO>(), It.IsAny<Role>()), Times.Never);
            _mockRoleRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetRoleDTO>(It.IsAny<Role>()), Times.Never);
        }
    }
}
