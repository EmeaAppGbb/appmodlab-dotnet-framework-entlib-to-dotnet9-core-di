using System.ComponentModel.DataAnnotations;

namespace HealthClaimsProcessor.Core.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }

        [Required, StringLength(100, ErrorMessage = "Provider name is required")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "NPI must be exactly 10 digits")]
        public string NPI { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}-\d{7}$", ErrorMessage = "Tax ID must be in format XX-XXXXXXX")]
        public string TaxId { get; set; }

        [Required, StringLength(50, ErrorMessage = "Specialty is required")]
        public string Specialty { get; set; }

        [Required, StringLength(100, ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required, StringLength(50, ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required, StringLength(2, MinimumLength = 2, ErrorMessage = "State must be 2 characters")]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "ZIP code must be in format XXXXX or XXXXX-XXXX")]
        public string ZipCode { get; set; }

        [Required]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Phone must be in format (XXX) XXX-XXXX")]
        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public bool IsNetworkProvider { get; set; }

        public DateTime CredentialedDate { get; set; }

        public DateTime? CredentialExpirationDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
