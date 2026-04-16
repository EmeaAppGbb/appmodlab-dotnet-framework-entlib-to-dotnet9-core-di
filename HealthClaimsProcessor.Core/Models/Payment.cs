using System.ComponentModel.DataAnnotations;

namespace HealthClaimsProcessor.Core.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Claim ID is required")]
        public int ClaimId { get; set; }

        public Claim Claim { get; set; }

        [Required, StringLength(50, ErrorMessage = "Payment number is required")]
        public string PaymentNumber { get; set; }

        [Range(0.01, 9999999.99, ErrorMessage = "Payment amount must be greater than 0")]
        public decimal PaymentAmount { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        public DateTime PaymentDate { get; set; }

        [Required, StringLength(20, ErrorMessage = "Payment method is required")]
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
