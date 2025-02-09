using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PMS_Project.Presenter.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class ProjectBoardController : BaseController<ProjectBoard, CreateProjectBoardDTO, ProjectBoardDTO, UpdateProjectBoardDTO>
    {
        private readonly IProjectBoardService _projectBoardService;
        private const string NameOfRoute = "GetProjectById";
        public ProjectBoardController(IProjectBoardService projectBoardService) : base(projectBoardService)
        {
            _projectBoardService = projectBoardService;
        }

        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(401)] // Unauthorized
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(500)] // Internal server error
        public override async Task<ActionResult<ProjectBoardDTO>> AddAsync([FromBody] CreateProjectBoardDTO createDTO)
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

                var projectBoard = await _projectBoardService.CreateNewProjectAsync(createDTO, userId);
                return CreatedAtRoute(NameOfRoute, new { id = projectBoard.Id }, projectBoard);
            }
            catch (ArgumentException ex)
            {
                // Handle argument exceptions, possibly due to invalid input
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle unauthorized access exceptions
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id:Guid}", Name = "GetProjectById")]
        /// <summary>
        /// Gets a project board by its ID.
        /// </summary>
        /// <param name="id">The ID of the project board.</param>
        /// <returns>The project board with the specified ID.</returns>
        public override async Task<ActionResult<ProjectBoardDTO>> GetByIdAsync(Guid id)
        {
            var projectBoard = await _projectBoardService.GetByIdAsync(id);
            if (projectBoard == null)
                return NotFound();
            return Ok(projectBoard);
        }

        [HttpGet("workspace/{workspaceId:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)] // Unauthorized
        [ProducesResponseType(403)] // Forbidden
        [ProducesResponseType(404)] // Not Found
        [ProducesResponseType(500)] // Internal Server Error
        public async Task<ActionResult<IEnumerable<ProjectBoardDTO>>> GetProjectsByWorkspaceIdAsync(Guid workspaceId)
        {
            try
            {
                // Get the current user's ID from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                    return Unauthorized("Invalid User ID format.");

                var projects = await _projectBoardService.GetProjectsByWorkspaceIdAsync(workspaceId, userId);

                if (!projects.Any())
                    return NotFound("No projects found in this workspace.");

                return Ok(projects);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An unexpected error occurred while retrieving projects.");
            }
        }


    }
}