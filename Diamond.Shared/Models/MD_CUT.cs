using System.ComponentModel.DataAnnotations;

namespace Diamond.Share.Models
{
    public class MD_CUT
    {

        [Key]
        public int CutId { get; set; }

        [Required, MaxLength(50)]
        public string CutName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Grade { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Range(0, 100)]
        public decimal? Score { get; set; }

    }
}
