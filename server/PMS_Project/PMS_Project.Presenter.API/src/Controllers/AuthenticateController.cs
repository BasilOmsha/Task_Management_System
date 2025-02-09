using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticateController : ControllerBase
    {

        private readonly IAppAuthService _appAuthService;

        private readonly IUserRepository _userRepo;


        public AuthenticateController(IAppAuthService appAuthService, IUserRepository userRepo)
        {
            _appAuthService = appAuthService;
            _userRepo = userRepo;
        }


        [HttpPost("login")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] AuthenticateDTO dto)
        {
            try
            {
                return Ok(await _appAuthService.AuthenticateAsync(dto));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]

        public async Task<ActionResult<TokenDTO>> Refresh([FromBody] TokenDTO dto)
        {
            try
            {
                // Step 1: Use the expired access token to get user ID:
                var principal = _appAuthService.GetPrincipalFromExpiredToken(dto.AccessToken!);

                // Step 2: Ensure we have a valid identity:
                if (principal?.Identity is not ClaimsIdentity identity)
                    return Unauthorized();
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();
                var userId = Guid.Parse(userIdClaim.Value);

                // Step 3: Retrieve the user from the database:
                var user = await _userRepo.GetByIdAsync(userId);
                if (user == null)
                    return Unauthorized();

                // Step 4: Verify the old refresh token is valid:
                if (user.RefreshToken == null ||
                    user.RefreshToken != dto.RefreshToken ||              // must match
                    user.RefreshTokenExpiry <= DateTime.UtcNow ||          // not expired
                    user.TokenRevoked == true)                             // not revoked
                {
                    return Unauthorized(); // or some appropriate error
                }

                // Step 5: If valid, issue new tokens:
                var newAccessToken = await _appAuthService.GenerateAccessToken(userId);
                var newRefreshToken = _appAuthService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                user.TokenRevoked = false;
                await _userRepo.UpdateAsync(user);

                return Ok(new TokenDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Authorize]
        [HttpDelete("logout")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<ActionResult<bool>> Logout()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Get the user id from the token
                if (userIdClaim == null)
                    return Unauthorized();

                var userId = Guid.Parse(userIdClaim.Value); // Parse the user id
                await _appAuthService.RevokeTokenAsync(userId); // Revoke the token
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}