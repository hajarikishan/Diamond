using System.ComponentModel.DataAnnotations;

namespace Diamond.Share.Models
{
    public class MD_CLARITY
    {

        [Key]
        public int ClarityId { get; set; }

        [Required(ErrorMessage = "Clarity name is required")]
        [MaxLength(50)]
        public string ClarityName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? Abbreviation { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int Rank { get; set; } = 0;

    }
}
