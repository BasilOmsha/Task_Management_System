using System.ComponentModel.DataAnnotations;

namespace PMS_Project.Application.DTOs
{
    public class AuthenticateDTO
    {
        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, MinLength(1)]
        public string? Password { get; set; }
    }

    public class TokenDTO
    {
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }

    }
}