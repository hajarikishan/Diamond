using System.ComponentModel.DataAnnotations;

namespace Diamond.Share.Models
{
    public class MD_COLOR
    {

        public int ColorId { get; set; }


        [Required(ErrorMessage = "Color name is required")]
        [StringLength(50, ErrorMessage = "Name too long (max 50 characters)")]
        public string ColorName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description too long (max 200 characters)")]
        public string? Description { get; set; }

    }
}
