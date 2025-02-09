using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;

namespace PMS_Project.Presenter.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class WorkspaceUserController : BaseController<Workspace_User, CreateWorkspaceUserDTO, GetWorkspaceUserDTO, UpdateWorkspaceUserDTO>
    {
        private const string NameOfRoute = "GetWorkspaceUserById";
        private readonly IWorkspaceUserService _workspaceUserService;
        public WorkspaceUserController(IWorkspaceUserService workspaceUserService) : base(workspaceUserService)
        {
            _workspaceUserService = workspaceUserService;
        }

        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(500)] // Internal server error
        public override async Task<ActionResult<GetWorkspaceUserDTO>> AddAsync([FromBody] CreateWorkspaceUserDTO createDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Get the current user's ID from the claims
                var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(currentUserIdClaim.Value, out Guid currentUserId))
                    return Unauthorized("Invalid User ID format.");

                // Call the service method
                var result = await _workspaceUserService.AddUserToWorkspaceAsync(createDTO, currentUserId);

                return Ok(new { message = "User added to workspace successfully.", data = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, ex.Message);
            }
        }

        //get all users of a given workspace
        [HttpGet("{workspaceId}/members")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(500)] // Internal server error

        public async Task<ActionResult<IEnumerable<GetWorkspaceUserDTO>>> GetAllUsers(Guid workspaceId)
        {
            try
            {
                var users = await _workspaceUserService.GetWorkspaceUsersAsync(workspaceId);
                return Ok(new { message = "Users retrieved successfully", data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}