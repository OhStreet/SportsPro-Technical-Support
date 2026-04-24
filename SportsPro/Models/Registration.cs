using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SportsPro.Models
{
    public class Registration
    {
        public int CustomerID { get; set; }
        [ValidateNever]
        public Customer Customer { get; set; } = null!;

        public int ProductID { get; set; }
        [ValidateNever]
        public Product Product { get; set; } = null!;
    }
}
