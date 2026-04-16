using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IPaymentRepository
    {
        List<Payment> GetAllPayments();
        Payment GetPaymentById(int paymentId);
        List<Payment> GetPaymentsByClaimId(int claimId);
        int InsertPayment(Payment payment);
        void UpdatePaymentStatus(int paymentId, string status, DateTime? clearedDate, string modifiedBy);
    }
}
