using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Abstractions.Repositories;

namespace PMS_Project.Presenter.API.Service
{
    public class AppAuthService : IAppAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepo;
        private readonly IPaswdService _paswdService;
        private readonly IJwtRsaKeysService _jwtRsaKeysService;

        public AppAuthService(IConfiguration configuration, IUserRepository userRepo, IPaswdService paswdService, IJwtRsaKeysService jwtRsaKeysService)
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _paswdService = paswdService;
            _jwtRsaKeysService = jwtRsaKeysService;
        }

        public async Task<TokenDTO> AuthenticateAsync(AuthenticateDTO userDTO)
        {
            // Get user by email
            var user = await _userRepo.GetUserByEmailAsync(userDTO.Email!) ?? throw new Exception("Invalid Credentials");

            // Verify password
            var isCorrectValid = _paswdService.VerifyPaswd(userDTO.Password!, user.Password!);

            if (!isCorrectValid)
                throw new Exception("Invalid Credentials");

            // check if user does't have a token or if it is expired
            if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiry <= DateTime.UtcNow || user.TokenRevoked == true)
            {
                // Generate a refresh token
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                user.UpdatedAt = DateTime.UtcNow;
                user.TokenRevoked = false;
                await _userRepo.UpdateAsync(user);
            }
            // access token
            var accessToken = await GenerateAccessToken(user.Id);

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };
        }

        /// <summary>AC
        /// Generates a new refresh token.
        /// </summary>
        /// <returns>A base64 encoded string representing the refresh token.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[256];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }

        public async Task<string> GenerateAccessToken(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id) ?? throw new Exception("User not found");
            var claims = new List<Claim>
            {   
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
            };
            // var privKey = _configuration["Jwt:Key"];
            // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privKey!));
            // var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var privateKey = _jwtRsaKeysService.SigningKey;
            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(privateKey),
                SecurityAlgorithms.RsaSha256);
            
            var tokenOptions = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                // Expires = DateTime.UtcNow.AddMinutes(1),
                // 40 seconds for testing
                // Expires = DateTime.UtcNow.AddSeconds(604800),
                // Expires = DateTime.UtcNow.AddMonths(3),
                SigningCredentials = signingCredentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(tokenOptions);

            return tokenHandler.WriteToken(accessToken);
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(_jwtRsaKeysService.ValidationKey)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<bool> RevokeTokenAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId) ?? throw new Exception("User not found");
            user.RefreshToken = null;
            user.RefreshTokenExpiry = DateTime.UtcNow;
            user.TokenRevoked = true;
            await _userRepo.UpdateAsync(user);
            return true;
        }
    }
}