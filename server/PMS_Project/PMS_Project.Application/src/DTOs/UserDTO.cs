using System.ComponentModel.DataAnnotations;

namespace PMS_Project.Application.DTOs
{
    public class CreateUserDTO
    {
        [Required, MinLength(2, ErrorMessage = "Firstname must be at least 2 characters long.")]
        public string? Firstname { get; set; }

        [Required, MinLength(2, ErrorMessage = "Lastname must be at least 2 characters long.")]
        public string? Lastname { get; set; }

        [Required, MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username must contain only letters and numbers.")]
        public string? Username { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string? Password { get; set; }

        [Required, Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }

    public class GetUserInfoDTO
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

    public class UpdateUserInfoDTO
    {

        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        public string? FirstName { get; set; }

        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
        public string? LastName { get; set; }

        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username must contain only letters and numbers.")]
        public string? Username { get; set; }


        public string? CurrentPassword { get; set; }


        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
                   ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    
        public string? NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        public string? ConfirmNewPassword { get; set; }


        [EmailAddress]
        public string? Email { get; set; }

    }


}