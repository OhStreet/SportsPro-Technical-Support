using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Incident
    {
        public int IncidentID { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be 3–100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be 5–2000 characters.")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateOpened { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? DateClosed { get; set; } = null;

        // FK validation
        [Range(1, int.MaxValue, ErrorMessage = "Customer is required.")]
        public int CustomerID { get; set; }                   // foreign key property
		public Customer? Customer { get; set; } = null!;       // navigation property

        [Range(1, int.MaxValue, ErrorMessage = "Product is required.")]
        public int ProductID { get; set; }                    // foreign key property
		public Product? Product { get; set; } = null!;         // navigation property

        [Range(1, int.MaxValue, ErrorMessage = "Technician is required.")]
        public int TechnicianID { get; set; }                 // foreign key property 
		public Technician? Technician { get; set; } = null!;   // navigation property

		
	}
}
