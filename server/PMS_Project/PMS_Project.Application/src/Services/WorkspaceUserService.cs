using AutoMapper;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.Common.Constants;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Services
{
    public class WorkspaceUserService : BaseService<Workspace_User, CreateWorkspaceUserDTO, GetWorkspaceUserDTO, UpdateWorkspaceUserDTO>, IWorkspaceUserService
    {
        private readonly IWorkspaceUserRepository _workspaceUserRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        // Parameterless constructor for proxy instantiation
        public WorkspaceUserService() : base(null, null) { }

        public WorkspaceUserService(IWorkspaceUserRepository workspaceUserRepo, IMapper mapper, IRoleRepository roleRepo, IUserRepository userRepo) : base(workspaceUserRepo, mapper)
        {
            _workspaceUserRepo = workspaceUserRepo;
            _mapper = mapper;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
        }

        public async Task AddOwnerUserToWorkspaceAsync(Guid workspaceId, Guid userId, Guid roleId)
        {
            Console.WriteLine("AddOwnerUserToWorkspaceAsync called with workspaceId: {0}, userId: {1}, roleId: {2}", workspaceId, userId, roleId);

            // Check if the user is already in the workspace
            var workspaceUser = await _workspaceUserRepo.GetWorkspaceUserAsync(userId, workspaceId);
            if (workspaceUser != null)
            {
                //Check if the role exists
                var role = await _workspaceUserRepo.GetUserRoleAsync(roleId) ?? throw new KeyNotFoundException("Role does not exist.");
                if (role == null)
                {
                    throw new KeyNotFoundException("Role does not exist.");
                }
                // check if the user has a role in the workspace
                if (workspaceUser.RoleId == null)
                {
                    workspaceUser.RoleId = roleId;
                    var updatedWorkspaceUser = _mapper.Map<Workspace_User>(workspaceUser);
                    await _workspaceUserRepo.UpdateAsync(updatedWorkspaceUser);
                }
                else
                {
                    throw new ArgumentException($"User already is a {role.Name} in the workspace.");
                }
            }
            else
            {
                // add the user to the workspace
                var newWorkspaceUser = new Workspace_User
                {
                    UserId = userId,
                    WorkspaceId = workspaceId,
                    RoleId = roleId,

                };

                await _workspaceUserRepo.AddAsync(newWorkspaceUser);
                Console.WriteLine("New Workspace_User entity added: {0}", newWorkspaceUser);
            }
        }

        public async Task<GetWorkspaceUserDTO> AddUserToWorkspaceAsync(CreateWorkspaceUserDTO createWorkspaceUserDTO, Guid currentUserId)
        {
            Console.WriteLine("Current user Id: {0}", currentUserId);

            var workspaceId = createWorkspaceUserDTO.WorkspaceId;
            var userIdToAdd = createWorkspaceUserDTO.UserId;
            var roleIdToAssign = createWorkspaceUserDTO.RoleId;

            Console.WriteLine("AddUserToWorkspaceAsync called with workspaceId: {0}, userIdToAdd: {1}, roleIdToAssign: {2}", workspaceId, userIdToAdd, roleIdToAssign);

            try
            {
                // 1. Check if the current user is part of the workspace
                var currentUserWorkspaceUser = await _workspaceUserRepo.GetWorkspaceUserAsync(currentUserId, workspaceId);
                if (currentUserWorkspaceUser == null)
                {
                    throw new UnauthorizedAccessException("You are not a member of this workspace.");
                }

                Console.WriteLine("Current user's role in the workspace: {0}", currentUserWorkspaceUser.RoleId);

                // 2. Get the role of the current user in the workspace
                var currentUserRole = await _roleRepo.GetByIdAsync(currentUserWorkspaceUser.RoleId);
                if (currentUserRole == null)
                {
                    throw new Exception("Your role in the workspace is not found.");
                }

                Console.WriteLine("Current user's role in the workspace: {0}", currentUserRole.Name);

                // 3. Check if the current user is an Owner or Admin
                if (currentUserRole.Name != Roles.Owner && currentUserRole.Name != Roles.Admin)
                {
                    throw new UnauthorizedAccessException("You do not have permission to add users to this workspace.");
                }

                Console.WriteLine("Current user is an Owner or Admin.");

                // 4. Get the role to assign
                var roleToAssign = await _roleRepo.GetByIdAsync(roleIdToAssign);
                if (roleToAssign == null)
                {
                    throw new KeyNotFoundException("The role you are trying to assign does not exist.");
                }

                Console.WriteLine("Role to assign: {0}", roleToAssign.Name);

                // 5. Owners and Admins cannot assign the Owner role
                if (roleToAssign.Name == Roles.Owner)
                {
                    throw new UnauthorizedAccessException("You cannot assign the 'Owner' role to other users.");
                }

                Console.WriteLine("Role to assign is not 'Owner'.");


                // Check if the user to add exists
                var userToAddExists = await _userRepo.GetByIdAsync(userIdToAdd);
                if (userToAddExists == null)
                {
                    throw new KeyNotFoundException("The user you are trying to add does not exist.");
                }

                // Check if the Id format is correct


                // 6. Check if the user to be added is already in the workspace
                var existingWorkspaceUser = await _workspaceUserRepo.GetWorkspaceUserAsync(userIdToAdd, workspaceId);
                if (existingWorkspaceUser != null)
                {
                    throw new ArgumentException("The user is already a member of this workspace.");
                }

                // 7. Add the user to the workspace
                var newWorkspaceUser = _mapper.Map<Workspace_User>(createWorkspaceUserDTO);

                await _workspaceUserRepo.AddAsync(newWorkspaceUser);
                Console.WriteLine("New user added to workspace: {0}", newWorkspaceUser);
                return _mapper.Map<GetWorkspaceUserDTO>(newWorkspaceUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<GetWorkspaceUserDTO>> GetWorkspaceUsersAsync(Guid workspaceId)
        {
            //get all users in the workspace'
            var workspaceUsers = await _workspaceUserRepo.GetAllUsers(workspaceId);
            return _mapper.Map<IEnumerable<GetWorkspaceUserDTO>>(workspaceUsers);

        }
    }
}