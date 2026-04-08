using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HealthClaimsProcessor.Core.Models
{
    public class ClaimLineItem
    {
        public int LineItemId { get; set; }

        public int ClaimId { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 999, RangeBoundaryType.Inclusive, 
            MessageTemplate = "Line number must be between 1 and 999")]
        public int LineNumber { get; set; }

        public DateTime ServiceDate { get; set; }

        [RegexValidator(@"^\d{5}$", MessageTemplate = "CPT code must be exactly 5 digits")]
        public string CPTCode { get; set; }

        public string CPTDescription { get; set; }

        [RegexValidator(@"^[A-Z]\d{2}(\.\d{1,2})?$", MessageTemplate = "ICD-10 code format is invalid")]
        public string ICD10Code { get; set; }

        public string ICD10Description { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 99, RangeBoundaryType.Inclusive,
            MessageTemplate = "Quantity must be between 1 and 99")]
        public int Quantity { get; set; }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "999999.99", RangeBoundaryType.Inclusive,
            MessageTemplate = "Unit charge must be between 0.01 and 999999.99")]
        public decimal UnitCharge { get; set; }

        public decimal TotalCharge { get; set; }

        public decimal ApprovedAmount { get; set; }

        public decimal PatientResponsibility { get; set; }

        public string DenialReason { get; set; }

        public string Notes { get; set; }
    }
}
