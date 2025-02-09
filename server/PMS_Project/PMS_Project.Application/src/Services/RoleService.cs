using AutoMapper;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Services
{
    public class RoleService : BaseService<Role, CreateRoleDTO, GetRoleDTO, UpdateRoleDTO>, IRoleService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepo, IMapper mapper) : base(roleRepo, mapper)
        {
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        public override async Task<GetRoleDTO> AddAsync(CreateRoleDTO createDTO)
        {
            if (createDTO == null)
                throw new ArgumentNullException(nameof(createDTO));

            // Check if a role with the same name already exists (case-insensitive)
            var existingRole = await _roleRepo.GetRoleByNameAsync(createDTO.Name);
            if (existingRole != null &&
                existingRole.Name.Equals(createDTO.Name, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Role already exists.");
            }

            // Mapping the DTO to the entity
            var role = _mapper.Map<Role>(createDTO);

            var newRole = await _roleRepo.AddAsync(role);
            return _mapper.Map<GetRoleDTO>(newRole);
        }

        public override async Task<GetRoleDTO> UpdateAsync(Guid id, UpdateRoleDTO updateDTO)
        {
            if (updateDTO == null)
                throw new ArgumentNullException(nameof(updateDTO));

            var roleExists = await _roleRepo.GetByIdAsync(id);
            if (roleExists == null)
                throw new KeyNotFoundException("Role does not exist.");

            // If updating the Name, ensure it's unique
            if (!string.IsNullOrEmpty(updateDTO.Name) &&
                !updateDTO.Name.Equals(roleExists.Name, StringComparison.OrdinalIgnoreCase))
            {
                var existingRoleName = await _roleRepo.GetRoleByNameAsync(updateDTO.Name);
                if (existingRoleName != null)
                    throw new ArgumentException("Another role with the same name already exists.");
            }

            // Proceed with the base update
            var updatedRole = await base.UpdateAsync(id, updateDTO);
            return updatedRole;
        }
    }
}