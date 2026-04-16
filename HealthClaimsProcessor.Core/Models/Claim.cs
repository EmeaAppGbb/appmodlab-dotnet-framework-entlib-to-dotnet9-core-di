using System.ComponentModel.DataAnnotations;

namespace HealthClaimsProcessor.Core.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Claim number is required")]
        [StringLength(50)]
        public string ClaimNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Provider ID is required")]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }

        [Required(ErrorMessage = "Service date is required")]
        public DateTime ServiceDateFrom { get; set; }

        [Required(ErrorMessage = "Service date is required")]
        public DateTime ServiceDateTo { get; set; }

        public DateTime SubmittedDate { get; set; }

        public ClaimStatus Status { get; set; }

        [Required(ErrorMessage = "Place of service is required")]
        [StringLength(20)]
        public string PlaceOfService { get; set; }

        public string DiagnosisPointer { get; set; }

        [Range(0.01, 9999999.99, ErrorMessage = "Total charge must be greater than 0")]
        public decimal TotalChargeAmount { get; set; }

        public decimal TotalApprovedAmount { get; set; }

        public decimal TotalPatientResponsibility { get; set; }

        public string PriorAuthorizationNumber { get; set; }

        public string ReferringProviderNPI { get; set; }

        public string AdmissionDate { get; set; }

        public string DischargeDate { get; set; }

        public string AccidentDate { get; set; }

        public bool IsAccidentRelated { get; set; }

        public bool IsEmploymentRelated { get; set; }

        public string ProcessedBy { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public string AdjudicationNotes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public List<ClaimLineItem> LineItems { get; set; }

        public Claim()
        {
            LineItems = new List<ClaimLineItem>();
            Status = ClaimStatus.Draft;
            CreatedDate = DateTime.Now;
        }
    }
}
