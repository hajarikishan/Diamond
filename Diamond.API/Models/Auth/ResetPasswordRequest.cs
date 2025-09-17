using System.ComponentModel.DataAnnotations;

namespace Diamond.API.Models.Auth
{
    public class ResetPasswordRequest
    {

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; } = string.Empty;

    }
}
