using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product code is required.")]
        [StringLength(20, ErrorMessage = "Product code must be 20 characters or less.")]
        public string ProductCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(50, ErrorMessage = "Product name must be 50 characters or less.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yearly price is required.")]
        [Range(0.01, 9999.99, ErrorMessage = "Yearly price must be greater than 0.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal YearlyPrice { get; set; }

        [Required(ErrorMessage = "Release date is required.")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
    }
}
