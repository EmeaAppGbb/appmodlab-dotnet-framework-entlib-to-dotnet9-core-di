using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IPaymentService
    {
        List<Payment> GetAllPayments();
        Payment GetPaymentById(int paymentId);
        int ProcessPayment(Payment payment);
        void ClearPayment(int paymentId, string modifiedBy);
    }
}
