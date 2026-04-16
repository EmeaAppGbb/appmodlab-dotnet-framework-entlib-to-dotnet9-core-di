using System.ComponentModel.DataAnnotations;

namespace HealthClaimsProcessor.Core.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required, StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters")]
        public string FirstName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "SSN must be in format XXX-XX-XXXX")]
        public string SSN { get; set; }

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

        public string Email { get; set; }

        [Required, StringLength(50, ErrorMessage = "Insurance policy number is required")]
        public string InsurancePolicyNumber { get; set; }

        [Required, StringLength(50, ErrorMessage = "Insurance group number is required")]
        public string InsuranceGroupNumber { get; set; }

        public DateTime InsuranceEffectiveDate { get; set; }
        
        public DateTime? InsuranceTerminationDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
