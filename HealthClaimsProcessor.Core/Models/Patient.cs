using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HealthClaimsProcessor.Core.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "First name must be between 1 and 50 characters")]
        public string FirstName { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "Last name must be between 1 and 50 characters")]
        public string LastName { get; set; }

        [NotNullValidator(MessageTemplate = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [RegexValidator(@"^\d{3}-\d{2}-\d{4}$", MessageTemplate = "SSN must be in format XXX-XX-XXXX")]
        public string SSN { get; set; }

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

        public string Email { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "Insurance policy number is required")]
        public string InsurancePolicyNumber { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "Insurance group number is required")]
        public string InsuranceGroupNumber { get; set; }

        public DateTime InsuranceEffectiveDate { get; set; }
        
        public DateTime? InsuranceTerminationDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
