using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HealthClaimsProcessor.Core.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, int.MaxValue, RangeBoundaryType.Inclusive,
            MessageTemplate = "Claim ID is required")]
        public int ClaimId { get; set; }

        public Claim Claim { get; set; }

        [StringLengthValidator(1, 50, MessageTemplate = "Payment number is required")]
        public string PaymentNumber { get; set; }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "9999999.99", RangeBoundaryType.Inclusive,
            MessageTemplate = "Payment amount must be greater than 0")]
        public decimal PaymentAmount { get; set; }

        [NotNullValidator(MessageTemplate = "Payment date is required")]
        public DateTime PaymentDate { get; set; }

        [StringLengthValidator(1, 20, MessageTemplate = "Payment method is required")]
        public string PaymentMethod { get; set; }

        public string CheckNumber { get; set; }

        public string EFTTraceNumber { get; set; }

        public string PayeeName { get; set; }

        public string PayeeAddress { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? ClearedDate { get; set; }

        public string RemittanceAdviceNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public Payment()
        {
            CreatedDate = DateTime.Now;
            PaymentStatus = "Pending";
        }
    }
}
