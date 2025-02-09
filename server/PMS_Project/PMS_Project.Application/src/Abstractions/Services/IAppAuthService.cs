using System.Security.Claims;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IAppAuthService
    {

        Task<TokenDTO> AuthenticateAsync(AuthenticateDTO model);
        Task<string> GenerateAccessToken(Guid id);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<bool> RevokeTokenAsync(Guid id);
        
    }
}