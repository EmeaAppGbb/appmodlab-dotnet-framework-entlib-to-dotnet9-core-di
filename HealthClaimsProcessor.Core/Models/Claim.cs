using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HealthClaimsProcessor.Core.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "Claim number is required")]
        public string ClaimNumber { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, int.MaxValue, RangeBoundaryType.Inclusive,
            MessageTemplate = "Patient ID is required")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, int.MaxValue, RangeBoundaryType.Inclusive,
            MessageTemplate = "Provider ID is required")]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }

        [NotNullValidator(MessageTemplate = "Service date is required")]
        public DateTime ServiceDateFrom { get; set; }

        [NotNullValidator(MessageTemplate = "Service date is required")]
        public DateTime ServiceDateTo { get; set; }

        public DateTime SubmittedDate { get; set; }

        public ClaimStatus Status { get; set; }

        [StringLengthValidator(1, 20, MessageTemplate = "Place of service is required")]
        public string PlaceOfService { get; set; }

        public string DiagnosisPointer { get; set; }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "9999999.99", RangeBoundaryType.Inclusive,
            MessageTemplate = "Total charge must be greater than 0")]
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

        [ObjectCollectionValidator(typeof(ClaimLineItem))]
        public List<ClaimLineItem> LineItems { get; set; }

        public Claim()
        {
            LineItems = new List<ClaimLineItem>();
            Status = ClaimStatus.Draft;
            CreatedDate = DateTime.Now;
        }
    }
}
