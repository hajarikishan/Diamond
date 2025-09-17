using System.ComponentModel.DataAnnotations;

namespace Diamond.API.Models.Auth
{
    public class RegisterRequest
    {

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Role { get; set; } = "User"; // default role

    }
}
