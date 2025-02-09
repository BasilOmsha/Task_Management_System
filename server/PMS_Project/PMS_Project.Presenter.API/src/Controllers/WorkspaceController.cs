using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PMS_Project.Domain.Models;

namespace PMS_Project.Presenter.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class WorkspaceController : BaseController<Workspace, CreateWorkspaceDTO, GetWorkspaceDTO, UpdateWorkspaceDTO>
    {
        private const string NameOfRoute = "GetWorkspaceById";
        private readonly IWorkspaceService _workspaceService;
        public WorkspaceController(IWorkspaceService workspaceService) : base(workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(500)] // Internal server error
        public override async Task<ActionResult<GetWorkspaceDTO>> AddAsync([FromBody] CreateWorkspaceDTO createDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Get the current user's ID
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                    return Unauthorized("Invalid User ID format.");

                try
                {
                    var workspace = await _workspaceService.AddWorkspaceAsync(createDTO, userId);
                    return CreatedAtRoute(NameOfRoute, new { id = workspace.Id }, workspace);
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
                {
                    return BadRequest(new { message = "A workspace with this name already exists" });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:Guid}", Name = NameOfRoute)]
        public override async Task<ActionResult<GetWorkspaceDTO>> GetByIdAsync(Guid id)
        {
            var workspace = await _workspaceService.GetByIdAsync(id);
            if (workspace == null)
                return NotFound();
            return Ok(workspace);
        }

        [HttpGet]
        public override async Task<ActionResult<IEnumerable<GetWorkspaceDTO>>> GetAllAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                    return Unauthorized("Invalid User ID format.");

                var workspaces = await _workspaceService.GetAllAsync(userId);
                return Ok(new { message = "Workspaces Retrieved", data = workspaces });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}