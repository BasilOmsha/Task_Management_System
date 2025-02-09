using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class UserController : BaseController<User, CreateUserDTO, GetUserInfoDTO, UpdateUserInfoDTO>
    {

        private readonly IUserService _userService;
        private readonly IAppAuthService _appAuthService;
        public UserController(IUserService userService, IAppAuthService appAuhtService) : base(userService)
        {
            _userService = userService;
            _appAuthService = appAuhtService;
        }

        //Get user by email
        [HttpGet("search")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public async Task<ActionResult<User>> GetUserByEmailAsync([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }
                return Ok(new { message = "User found", data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Something went wrong!" });
            }
        }

        //Get only loggedin user info
        [Authorize]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        public override async Task<ActionResult<GetUserInfoDTO>> GetByIdAsync([FromRoute] Guid id)
        {
            try
            {
                // check if the param id is the same as the logged in user id
                var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(currentUserIdClaim.Value, out Guid currentUserId))
                    return Unauthorized("Invalid User ID format.");

                // if (id != currentUserId)
                //     return Unauthorized("You are not authorized to view this user's information.");

                var entity = await _userService.GetByIdAsync(id);
                return Ok(new { message = "Entity Retrieved", data = entity });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Update only loggedin user info
        [Authorize]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 405)] // methode not allowed
        [ProducesResponseType(statusCode: 500)]
        public override async Task<ActionResult<GetUserInfoDTO>> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserInfoDTO updateDTO)
        {
            try
            {
                // check if the param id is the same as the logged in user id
                var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(currentUserIdClaim.Value, out Guid currentUserId))
                    return Unauthorized("Invalid User ID format.");

                if (id != currentUserId)
                    return Unauthorized("You are not authorized to update this user's information.");

                var entity = await _userService.UpdateAsync(id, updateDTO);
                return Ok(new { message = "Your info Updated successfuly", data = entity });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Delete only loggedin user info
        [Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 405)] // methode not allowed
        [ProducesResponseType(statusCode: 500)]
        public override async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                // check if the param id is the same as the logged in user id
                var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!Guid.TryParse(currentUserIdClaim.Value, out Guid currentUserId))
                    return Unauthorized("Invalid User ID format.");

                if (id != currentUserId)
                    return Unauthorized("You are not authorized to delete this user's information.");
                // get user refresh token
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                var result = await _userService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new { message = "Your info Deleted successfuly" });
                }
                return StatusCode(500, "An error occurred while deleting the entity");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}