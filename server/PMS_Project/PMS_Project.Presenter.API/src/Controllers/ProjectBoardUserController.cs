using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;
using PMS_Project.Presenter.API.Controllers;


namespace PMS_Project.Presenter.APIrc.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class ProjectBoardUserController : BaseController<ProjectBoard_User, CreateProjectBoardUserDTO, GetProjectBoardUserDTO, CreateProjectBoardUserDTO>
    {
        private readonly IProjectBoardUserService _projectBoardUserService;
        public ProjectBoardUserController(IProjectBoardUserService projectBoardUserService) : base(projectBoardUserService)
        {
            _projectBoardUserService = projectBoardUserService;
        }
        

        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(401)] // Unauthorized 
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(500)] // Internal server error
        public override async Task<ActionResult<GetProjectBoardUserDTO>> AddAsync([FromBody] CreateProjectBoardUserDTO createDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Console.WriteLine("AddAsync called with createDTO: {0}", createDTO);
                // Get the current user's ID from the claims
                var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(currentUserIdClaim.Value, out Guid currentUserId))
                    return Unauthorized("Invalid User ID format.");

                // Call the service method
                var result = await _projectBoardUserService.AddUserToProjectBoardAsync(createDTO, currentUserId);

                return Ok(new { message = "User added to project board successfully.", data = result });
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

    }
}