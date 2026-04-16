using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Data;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly HealthClaimsDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(HealthClaimsDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Payment> GetAllPayments()
        {
            _logger.LogInformation("Retrieving all payments");

            try
            {
                var payments = _context.Payments
                    .Include(p => p.Claim)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} payments", payments.Count);
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payments");
                throw;
            }
        }

        public Payment GetPaymentById(int paymentId)
        {
            _logger.LogInformation("Retrieving payment with ID: {PaymentId}", paymentId);

            try
            {
                var payment = _context.Payments
                    .Include(p => p.Claim)
                    .FirstOrDefault(p => p.PaymentId == paymentId);

                if (payment != null)
                {
                    _logger.LogInformation("Found payment: {PaymentNumber}", payment.PaymentNumber);
                }
                else
                {
                    _logger.LogWarning("Payment with ID {PaymentId} not found", paymentId);
                }

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment with ID: {PaymentId}", paymentId);
                throw;
            }
        }

        public List<Payment> GetPaymentsByClaimId(int claimId)
        {
            _logger.LogInformation("Retrieving payments for claim ID: {ClaimId}", claimId);

            try
            {
                var payments = _context.Payments
                    .Where(p => p.ClaimId == claimId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} payments for claim ID: {ClaimId}", payments.Count, claimId);
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments for claim ID: {ClaimId}", claimId);
                throw;
            }
        }

        public int InsertPayment(Payment payment)
        {
            _logger.LogInformation("Inserting new payment: {PaymentNumber} for claim ID: {ClaimId}", payment.PaymentNumber, payment.ClaimId);

            try
            {
                _context.Payments.Add(payment);
                _context.SaveChanges();

                _logger.LogInformation("Inserted payment with ID: {PaymentId}", payment.PaymentId);
                return payment.PaymentId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting payment: {PaymentNumber}", payment.PaymentNumber);
                throw;
            }
        }

        public void UpdatePaymentStatus(int paymentId, string status, DateTime? clearedDate, string modifiedBy)
        {
            _logger.LogInformation("Updating payment status for payment ID: {PaymentId} to {Status}", paymentId, status);

            try
            {
                var payment = _context.Payments.Find(paymentId);
                if (payment != null)
                {
                    payment.PaymentStatus = status;
                    payment.ClearedDate = clearedDate;
                    payment.ModifiedBy = modifiedBy;
                    payment.ModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                }

                _logger.LogInformation("Updated payment ID: {PaymentId} status to {Status}", paymentId, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status for payment ID: {PaymentId}", paymentId);
                throw;
            }
        }
    }
}
