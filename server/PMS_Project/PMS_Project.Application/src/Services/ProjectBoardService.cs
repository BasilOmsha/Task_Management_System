using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Abstractions.Services;
using AutoMapper;
using PMS_Project.Application.Common.Constants;

namespace PMS_Project.Application.Services
{
    public class ProjectBoardService : BaseService<ProjectBoard, CreateProjectBoardDTO, ProjectBoardDTO, UpdateProjectBoardDTO>, IProjectBoardService
    {
        private readonly IProjectBoardRepository _projectBoardRepository;
        private readonly IWorkspaceService _workspaceService;
        private readonly IWorkspaceUserRepository _workspaceUserRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IWorkspaceRepository _workspaceRepository;

        public ProjectBoardService(IWorkspaceRepository workspaceRepository, IProjectBoardRepository projectBoardRepository, IWorkspaceService workspaceService, IMapper mapper, IWorkspaceUserRepository workspaceUserRepo, IRoleRepository roleRepository)
        : base(projectBoardRepository, mapper)
        {
            _projectBoardRepository = projectBoardRepository;
            _workspaceService = workspaceService;
            _mapper = mapper;
            _workspaceUserRepository = workspaceUserRepo;
            _roleRepository = roleRepository;
            _workspaceRepository = workspaceRepository;
            //return projectBoardDTO;
        }

        public async Task<IEnumerable<ProjectBoardDTO>> GetProjectsByWorkspaceIdAsync(Guid workspaceId, Guid userId)
        {
            Console.WriteLine("WorkspaceId: {0}", workspaceId);
            // Check if user is part of the workspace
            var workspaceUser = await _workspaceUserRepository.GetWorkspaceUserAsync(userId, workspaceId);
            if (workspaceUser == null)
            {
                throw new UnauthorizedAccessException("User is not part of the workspace.");
            }
            Console.WriteLine("WorkspaceUserRoleId: {0}", workspaceUser.RoleId);

            // Get workspace to check if it exists
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId) ?? throw new KeyNotFoundException("Workspace does not exist.");
            Console.WriteLine("Workspace: {0}", workspace.Name);
            // Get user's role in the workspace
            var userRole = await _roleRepository.GetByIdAsync(workspaceUser.RoleId);

            // Get all projects in the workspace
            var projects = await _projectBoardRepository.GetProjectsByWorkspaceIdAsync(workspaceId);
            Console.WriteLine("Projects: {0}", projects);

            // Filter projects based on user's role and project visibility
            var accessibleProjects = projects.Where(project =>
                // If user is Admin or Owner, they can see all projects
                userRole.Name == Roles.Admin || userRole.Name == Roles.Owner || userRole.Name == Roles.Contributor ||
                // Otherwise, user can only see public projects or projects they created
                project.IsPublic || project.CreatorUserId == userId
            );

            return _mapper.Map<IEnumerable<ProjectBoardDTO>>(accessibleProjects);
        }
        public async Task<ProjectBoardDTO> CreateNewProjectAsync(CreateProjectBoardDTO createProjectBoard, Guid userId)
        {
            var newProject = _mapper.Map<ProjectBoard>(createProjectBoard);
            newProject.CreatorUserId = userId;
            newProject.CreatedAt = DateTime.UtcNow;
            newProject.UpdatedAt = DateTime.UtcNow;

            //To which workspace does the project board belong?
            var workspaceId = newProject.WorkspaceId;

            Console.WriteLine("WorkspaceId: {0}", workspaceId);
            //Does the workspace exist?
            var workspace = await _workspaceService.GetByIdAsync(workspaceId);
            if (workspace == null)
            {
                throw new KeyNotFoundException("Workspace does not exist.");
            }

            //Are you part of the workspace?
            var workspaceUser = await _workspaceUserRepository.GetWorkspaceUserAsync(userId, workspaceId);
            if (workspaceUser == null)
            {
                throw new KeyNotFoundException("User is not part of the workspace.");
            }

            Console.WriteLine("Check WorkspaceUserRole: {0}", workspaceUser.RoleId);


            // Are you an admin or owner of the workspace?
            var roleName = await _roleRepository.GetByIdAsync(workspaceUser.RoleId);
            if (roleName.Name != Roles.Admin && roleName.Name != Roles.Owner)
            {
                throw new UnauthorizedAccessException($"As a '{roleName.Name}' you do not have the required permissions to create a project board.");
            }

            Console.WriteLine("RoleName: {0}", roleName.Name);

            //Is the project board name unique?
            bool projectBoard = await _projectBoardRepository.GetProjectBoardByNameAsync(newProject.Name, workspaceId);
            if (!projectBoard)
            {
                throw new ArgumentException("Project board name already exists in the workspace.");
            }

            Console.WriteLine("ProjectBoard: {0}", projectBoard);

            // create the project board
            await _projectBoardRepository.AddAsync(newProject);
            return _mapper.Map<ProjectBoardDTO>(newProject);

        }

        public override async Task<ProjectBoardDTO> UpdateAsync(Guid id, UpdateProjectBoardDTO projectBoardDTO)
        {
            var projectBoard = new ProjectBoard
            {
                WorkspaceId = projectBoardDTO.WorkspaceId,
                Name = projectBoardDTO.Name,
                Description = projectBoardDTO.Description,
                IsPublic = projectBoardDTO.IsPublic,
            };

            await _projectBoardRepository.UpdateAsync(projectBoard);
            return _mapper.Map<ProjectBoardDTO>(projectBoard);
        }
        public override async Task<bool> DeleteAsync(Guid id)
        {
            var projectBoard = await _projectBoardRepository.GetByIdAsync(id);
            if (projectBoard != null)
            {
                await _workspaceService.RemoveProjectBoardAsync(projectBoard.WorkspaceId, projectBoard);
                await _projectBoardRepository.DeleteAsync(id);
                return true;
            }
            return false;
        }


    }
}