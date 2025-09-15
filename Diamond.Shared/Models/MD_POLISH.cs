using System.ComponentModel.DataAnnotations;

namespace Diamond.Share.Models
{
    public class MD_POLISH
    {

        [Key]
        public int PolishId { get; set; }

        [Required, MaxLength(50)]
        public string PolishName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public int Rank { get; set; } = 0;

    }
}
