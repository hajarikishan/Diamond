using System.ComponentModel.DataAnnotations;

namespace Diamond.Share.Models
{
    public class MD_USER
    {

        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "User";

        public bool IsEmailConfirmed { get; set; } = false;


        [MaxLength(500)]
        public string? ProfilePicturePath { get; set; }


        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
