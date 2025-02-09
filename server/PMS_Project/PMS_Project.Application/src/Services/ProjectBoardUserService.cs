using AutoMapper;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.Common.Constants;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;


namespace PMS_Project.Application.Services
{
    public class ProjectBoardUserService : BaseService<ProjectBoard_User, CreateProjectBoardUserDTO, GetProjectBoardUserDTO, CreateProjectBoardUserDTO>, IProjectBoardUserService
    {
        private readonly IProjectBoardUserRepository _projectBoardUserRepo;
        private readonly IWorkspaceUserRepository _workspaceUserRepo;
        private readonly IProjectBoardRepository _projectBoardRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public ProjectBoardUserService(IProjectBoardUserRepository projectBoardUserRepo, IMapper mapper, IWorkspaceUserRepository workspaceUserRepo, IProjectBoardRepository projectBoardRepository, IRoleRepository roleRepo) : base(projectBoardUserRepo, mapper)
        {
            _projectBoardUserRepo = projectBoardUserRepo;
            _workspaceUserRepo = workspaceUserRepo;
            _projectBoardRepository = projectBoardRepository;
            _roleRepository = roleRepo;
            _mapper = mapper;
        }

        public async Task<GetProjectBoardUserDTO> AddUserToProjectBoardAsync(CreateProjectBoardUserDTO createProjectBoardUserDTO, Guid currentUserId)
        {   
            Console.WriteLine("AddUserToProjectBoardAsync called with createProjectBoardUserDTO: {0}, currentUserId: {1}", createProjectBoardUserDTO, currentUserId);
            // map the dto to the model
            var projectBoardUser = _mapper.Map<ProjectBoard_User>(createProjectBoardUserDTO);

            try
            {
                // To which project board are you adding the user?
                var projectBoardId = projectBoardUser.ProjectBoardId;

                Console.WriteLine("ProjectBoardId: {0}", projectBoardId);

                // check if the project board exists
                var projWorkspace = await _projectBoardRepository.GetByIdAsync(projectBoardId);
                if (projWorkspace == null)
                {
                    throw new KeyNotFoundException("Project board does not exist.");
                }

                // get the workspace id from the project board
                var workspaceId = projWorkspace.WorkspaceId;
               
                // Check if the current user is part of the workspace
                var currentUserWorkspaceUser = await _workspaceUserRepo.GetWorkspaceUserAsync(currentUserId, workspaceId);
                if (currentUserWorkspaceUser == null)
                {
                    throw new UnauthorizedAccessException("You are not a member of this workspace.");
                }

                // Check if the user to be added is part of the workspace
                var userWorkspaceUser = await _workspaceUserRepo.GetWorkspaceUserAsync(projectBoardUser.UserId, workspaceId);
                if (userWorkspaceUser == null)
                {
                    throw new KeyNotFoundException("User is not part of the workspace.");
                }

                // check if the current user is an admin or owner of the workspace
                var roleName = await _roleRepository.GetByIdAsync(currentUserWorkspaceUser.RoleId);
                if (roleName.Name != Roles.Admin && roleName.Name != Roles.Owner)
                {
                    throw new UnauthorizedAccessException($"You as a/an {roleName.Name} do not have the permission to add a user to the project board.");
                }

                // Check if the user is already in the project board
                var projectBoardUserExists = await _projectBoardUserRepo.GetProjectBoardUserAsync(projectBoardUser.UserId, projectBoardUser.ProjectBoardId);
                if (!projectBoardUserExists)
                {
                    throw new ArgumentException("User is already in the project board.");
                }

                // Add the user to the project board
                await _projectBoardUserRepo.AddAsync(projectBoardUser);
                return _mapper.Map<GetProjectBoardUserDTO>(projectBoardUser);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}