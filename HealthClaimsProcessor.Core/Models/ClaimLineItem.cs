using System.ComponentModel.DataAnnotations;

namespace HealthClaimsProcessor.Core.Models
{
    public class ClaimLineItem
    {
        public int LineItemId { get; set; }

        public int ClaimId { get; set; }

        [Range(1, 999, ErrorMessage = "Line number must be between 1 and 999")]
        public int LineNumber { get; set; }

        public DateTime ServiceDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "CPT code must be exactly 5 digits")]
        public string CPTCode { get; set; }

        public string CPTDescription { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]\d{2}(\.\d{1,2})?$", ErrorMessage = "ICD-10 code format is invalid")]
        public string ICD10Code { get; set; }

        public string ICD10Description { get; set; }

        [Range(1, 99, ErrorMessage = "Quantity must be between 1 and 99")]
        public int Quantity { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "Unit charge must be between 0.01 and 999999.99")]
        public decimal UnitCharge { get; set; }

        public decimal TotalCharge { get; set; }

        public decimal ApprovedAmount { get; set; }

        public decimal PatientResponsibility { get; set; }

        public string DenialReason { get; set; }

        public string Notes { get; set; }
    }
}
