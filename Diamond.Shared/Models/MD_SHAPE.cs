using System.ComponentModel.DataAnnotations;

namespace Diamond.Share.Models
{
    public class MD_SHAPE
    {
        [Key]
        public int ShapeId { get; set; }

        [Required(ErrorMessage = "Shape name is required")]
        [MaxLength(50)]
        public string ShapeName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "MinCarat must be greater than 0")]
        public decimal MinCarat { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "MaxCarat must be greater than 0")]
        public decimal MaxCarat { get; set; }

    }
}
