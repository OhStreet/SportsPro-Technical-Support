using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 50 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "City must be between 1 and 50 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "State must be between 1 and 50 characters.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Postal code must be between 1 and 20 characters.")]
        public string PostalCode { get; set; } = string.Empty;

        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$",
            ErrorMessage = "Phone must be in (999) 999-9999 format.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Email must be between 1 and 50 characters.")]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckEmail", "Validation", AdditionalFields = "CustomerID",
            ErrorMessage = "This email address is already used by another customer.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        public string CountryID { get; set; } = string.Empty;

        // ValidateNever prevents model binding from requiring Country navigation property
        [ValidateNever]
        public Country Country { get; set; } = null!;

        public string FullName => FirstName + " " + LastName;

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
