using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class AuthRegisterRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; } // This property represents the username of the user registering for an account. It is marked as required, meaning it must be provided in the request.


        [Required]
        [MinLength(8)]
        [RegularExpression("(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).*")]
        public required string Password { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress]
        public required string Email { get; set; }






    }

    public class AuthLoginRequestDto
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
