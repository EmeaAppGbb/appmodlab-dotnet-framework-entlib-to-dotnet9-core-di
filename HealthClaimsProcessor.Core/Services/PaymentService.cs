using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClaimRepository _claimRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository, IClaimRepository claimRepository, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _claimRepository = claimRepository;
            _logger = logger;
        }

        public List<Payment> GetAllPayments()
        {
            _logger.LogInformation("Getting all payments");

            try
            {
                return _paymentRepository.GetAllPayments();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payments");
                throw;
            }
        }

        public Payment GetPaymentById(int paymentId)
        {
            _logger.LogInformation("Getting payment by ID: {PaymentId}", paymentId);

            try
            {
                return _paymentRepository.GetPaymentById(paymentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment with ID: {PaymentId}", paymentId);
                throw;
            }
        }

        public int ProcessPayment(Payment payment)
        {
            _logger.LogInformation("Processing payment for claim ID: {ClaimId}", payment.ClaimId);

            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(payment, new ValidationContext(payment), validationResults, true))
                {
                    var messages = validationResults.Select(r => r.ErrorMessage).ToList();
                    string errorMessage = "Payment validation failed: " + string.Join("; ", messages);
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                var claim = _claimRepository.GetClaimById(payment.ClaimId);
                if (claim == null)
                {
                    throw new InvalidOperationException($"Claim with ID {payment.ClaimId} not found");
                }

                if (claim.Status != ClaimStatus.Approved && claim.Status != ClaimStatus.PartiallyApproved)
                {
                    throw new InvalidOperationException(
                        $"Claim with ID {payment.ClaimId} is not in an approved status. Current status: {claim.Status}");
                }

                payment.PaymentNumber = GeneratePaymentNumber();
                payment.CreatedDate = DateTime.Now;
                payment.PaymentStatus = "Processed";

                int paymentId = _paymentRepository.InsertPayment(payment);

                _claimRepository.UpdateClaimStatus(payment.ClaimId, ClaimStatus.Paid, "PaymentService",
                    $"Payment processed. PaymentNumber: {payment.PaymentNumber}");

                _logger.LogInformation(
                    "Payment processed successfully with ID: {PaymentId}, PaymentNumber: {PaymentNumber}",
                    paymentId, payment.PaymentNumber);

                return paymentId;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for claim ID: {ClaimId}", payment.ClaimId);
                throw;
            }
        }

        public void ClearPayment(int paymentId, string modifiedBy)
        {
            _logger.LogInformation("Clearing payment with ID: {PaymentId}", paymentId);

            try
            {
                var payment = _paymentRepository.GetPaymentById(paymentId);
                if (payment == null)
                {
                    throw new InvalidOperationException($"Payment with ID {paymentId} not found");
                }

                if (payment.PaymentStatus != "Processed" && payment.PaymentStatus != "Pending")
                {
                    throw new InvalidOperationException(
                        $"Payment with ID {paymentId} cannot be cleared. Current status: {payment.PaymentStatus}");
                }

                _paymentRepository.UpdatePaymentStatus(paymentId, "Cleared", DateTime.Now, modifiedBy);
                _logger.LogInformation("Payment with ID: {PaymentId} cleared successfully", paymentId);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing payment with ID: {PaymentId}", paymentId);
                throw;
            }
        }

        private string GeneratePaymentNumber()
        {
            return $"PAY-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }
    }
}
