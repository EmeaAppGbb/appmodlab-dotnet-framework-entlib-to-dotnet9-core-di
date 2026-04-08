using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HealthClaimsProcessor.Core.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }

        [StringLengthValidator(1, 100, MessageTemplate = "Provider name is required")]
        public string Name { get; set; }

        [RegexValidator(@"^\d{10}$", MessageTemplate = "NPI must be exactly 10 digits")]
        public string NPI { get; set; }

        [RegexValidator(@"^\d{2}-\d{7}$", MessageTemplate = "Tax ID must be in format XX-XXXXXXX")]
        public string TaxId { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "Specialty is required")]
        public string Specialty { get; set; }

        [StringLengthValidator(1, 100, MessageTemplate = "Address is required")]
        public string Address { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "City is required")]
        public string City { get; set; }

        [StringLengthValidator(2, 2, MessageTemplate = "State must be 2 characters")]
        public string State { get; set; }

        [RegexValidator(@"^\d{5}(-\d{4})?$", MessageTemplate = "ZIP code must be in format XXXXX or XXXXX-XXXX")]
        public string ZipCode { get; set; }

        [RegexValidator(@"^\(\d{3}\) \d{3}-\d{4}$", MessageTemplate = "Phone must be in format (XXX) XXX-XXXX")]
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
