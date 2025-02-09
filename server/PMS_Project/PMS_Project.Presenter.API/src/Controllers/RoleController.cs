using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;
using PMS_Project.Presenter.API.Controllers;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class RoleController : BaseController<Role, CreateRoleDTO, GetRoleDTO, UpdateRoleDTO>
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) : base(roleService)
        {
            _roleService = roleService;
        }
    }
}