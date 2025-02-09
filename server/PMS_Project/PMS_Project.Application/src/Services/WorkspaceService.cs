using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.Common.Constants;

namespace PMS_Project.Application.Services
{
    public class WorkspaceService : IWorkspaceService, IBaseService<CreateWorkspaceDTO, GetWorkspaceDTO, UpdateWorkspaceDTO>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkspaceUserService _workspaceUserService;
        private readonly IRoleRepository _roleRepository;

        public WorkspaceService(IWorkspaceRepository workspaceRepository, IRoleRepository roleRepository, IWorkspaceUserService workspaceUserService)
        {
            _workspaceRepository = workspaceRepository;
            _roleRepository = roleRepository;
            _workspaceUserService = workspaceUserService;
        }

        public async Task<IEnumerable<GetWorkspaceDTO>> GetAllAsync(Guid userId)
        {
            var workspaces = await _workspaceRepository.GetWorkspacesForUserAsync(userId);
            return workspaces.Select(workspace => new GetWorkspaceDTO
            {
                Id = workspace.Id,
                Name = workspace.Name,
                CreatorUserId = workspace.CreatorUserId,
                Description = workspace.Description,
                IsPublic = workspace.IsPublic,
                CreatedAt = workspace.CreatedAt,
                UpdatedAt = workspace.UpdatedAt,
                CreatorUsername = workspace.User?.Username,
                WorkspaceUsers = workspace.Workspace_Users?.Select(wu => new WorkspaceUserDTO
                {
                    Id = wu.WorkspaceId,
                    UserId = wu.UserId,
                    Username = wu.User?.Username,
                    RoleId = wu.RoleId,
                    RoleName = wu.Role?.Name
                }).ToList()
            });
        }

        public async Task<GetWorkspaceDTO> GetByIdAsync(Guid id)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(id);
            Console.WriteLine("Hello: {0}", workspace.Name);
            return new GetWorkspaceDTO
            {
                Id = workspace.Id,
                Name = workspace.Name,
                CreatorUserId = workspace.CreatorUserId,
                Description = workspace.Description,
                IsPublic = workspace.IsPublic,
                CreatedAt = workspace.CreatedAt,
                UpdatedAt = workspace.UpdatedAt,
                CreatorUsername = workspace.User?.Username,
            };
        }

        public async Task<IEnumerable<GetWorkspaceDTO>> GetAllAsync()
        {
            throw new NotImplementedException("Use GetAllAsync(userId) instead");
            // var workspaces = await _workspaceRepository.GetAllAsync();
            // return workspaces.Select(workspace => new GetWorkspaceDTO
            // {
            //     Id = workspace.Id,
            //     Name = workspace.Name,
            //     CreatorUserId = workspace.CreatorUserId,
            //     Description = workspace.Description,
            //     IsPublic = workspace.IsPublic,
            //     CreatedAt = workspace.CreatedAt,
            //     UpdatedAt = workspace.UpdatedAt
            // });
        }

        public async Task<GetWorkspaceDTO> AddWorkspaceAsync(CreateWorkspaceDTO workspaceDto, Guid userId)
        {
            // Check if workspace with same name exists for this user
            var existingWorkspace = await _workspaceRepository.GetByNameAndCreatorAsync(
                workspaceDto.Name,
                userId
            );

            if (existingWorkspace != null)
            {
                throw new InvalidOperationException("A workspace with this name already exists");
            }

            var workspace = new Workspace
            {
                Name = workspaceDto.Name,
                CreatorUserId = userId,
                Description = workspaceDto.Description,
                IsPublic = workspaceDto.IsPublic,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _workspaceRepository.AddAsync(workspace);

            //get the owner role Id
            var ownerRole = await _roleRepository.GetRoleByNameAsync(Roles.Owner);

            await _workspaceUserService.AddOwnerUserToWorkspaceAsync(workspace.Id, workspace.CreatorUserId, ownerRole.Id);

            return new GetWorkspaceDTO
            {
                Id = workspace.Id,
                Name = workspace.Name,
                CreatorUserId = workspace.CreatorUserId,
                Description = workspace.Description,
                IsPublic = workspace.IsPublic,
                CreatedAt = workspace.CreatedAt,
                UpdatedAt = workspace.UpdatedAt
            };
        }

        public async Task<GetWorkspaceDTO> AddAsync(CreateWorkspaceDTO workspaceDto)
        {
            var workspace = new Workspace
            {
                Name = workspaceDto.Name,
                CreatorUserId = Guid.NewGuid(), //should be the current user's id - please fix this
                Description = workspaceDto.Description,
                IsPublic = workspaceDto.IsPublic,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _workspaceRepository.AddAsync(workspace);

            return new GetWorkspaceDTO
            {
                Id = workspace.Id,
                Name = workspace.Name,
                CreatorUserId = workspace.CreatorUserId,
                Description = workspace.Description,
                IsPublic = workspace.IsPublic,
                CreatedAt = workspace.CreatedAt,
                UpdatedAt = workspace.UpdatedAt
            };
        }

        public async Task<GetWorkspaceDTO> UpdateAsync(Guid id, UpdateWorkspaceDTO workspaceDto)
        {
            var workspace = new Workspace
            {
                Name = workspaceDto.Name,
                Description = workspaceDto.Description,
                IsPublic = workspaceDto.IsPublic,
                UpdatedAt = workspaceDto.UpdatedAt
            };
            await _workspaceRepository.UpdateAsync(workspace);

            return new GetWorkspaceDTO
            {
                Id = workspace.Id,
                Name = workspace.Name,
                CreatorUserId = workspace.CreatorUserId,
                Description = workspace.Description,
                IsPublic = workspace.IsPublic,
                CreatedAt = workspace.CreatedAt,
                UpdatedAt = workspace.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _workspaceRepository.DeleteAsync(id);
            return true;
        }

        public async Task AddProjectBoardAsync(Guid workspaceId, ProjectBoard projectBoard)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
            workspace.ProjectBoards?.Add(projectBoard);
            await _workspaceRepository.UpdateAsync(workspace);
        }

        public async Task RemoveProjectBoardAsync(Guid workspaceId, ProjectBoard projectBoard)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
            workspace.ProjectBoards?.Remove(projectBoard);
            await _workspaceRepository.UpdateAsync(workspace);
        }
    }
}