using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using HealthClaimsProcessor.Core.DataAccess;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly ClaimRepository _claimRepository;
        private readonly ExceptionManager _exceptionManager;

        public PaymentService(PaymentRepository paymentRepository, ClaimRepository claimRepository)
        {
            _paymentRepository = paymentRepository;
            _claimRepository = claimRepository;
            _exceptionManager = new ExceptionPolicyFactory(new SystemConfigurationSource()).CreateManager();
        }

        public List<Payment> GetAllPayments()
        {
            LoggingHelper.LogInfo("Getting all payments", "Service");

            try
            {
                return _paymentRepository.GetAllPayments();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all payments", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return new List<Payment>();
            }
        }

        public Payment GetPaymentById(int paymentId)
        {
            LoggingHelper.LogInfo($"Getting payment by ID: {paymentId}", "Service");

            try
            {
                return _paymentRepository.GetPaymentById(paymentId);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving payment with ID: {paymentId}", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return null;
            }
        }

        public int ProcessPayment(Payment payment)
        {
            LoggingHelper.LogInfo($"Processing payment for claim ID: {payment.ClaimId}", "Service");

            try
            {
                var validator = ValidationFactory.CreateValidator<Payment>();
                var validationResults = validator.Validate(payment);
                if (!validationResults.IsValid)
                {
                    var messages = new List<string>();
                    foreach (var result in validationResults)
                    {
                        messages.Add(result.Message);
                    }
                    string errorMessage = "Payment validation failed: " + string.Join("; ", messages);
                    LoggingHelper.LogWarning(errorMessage, "Service");
                    throw new ValidationException(errorMessage);
                }

                // Verify the claim exists and is in an approved status
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

                // Generate payment number
                payment.PaymentNumber = GeneratePaymentNumber();
                payment.CreatedDate = DateTime.Now;
                payment.PaymentStatus = "Processed";

                int paymentId = _paymentRepository.InsertPayment(payment);

                // Update claim status to Paid
                _claimRepository.UpdateClaimStatus(payment.ClaimId, ClaimStatus.Paid, "PaymentService", 
                    $"Payment processed. PaymentNumber: {payment.PaymentNumber}");

                LoggingHelper.LogInfo(
                    $"Payment processed successfully with ID: {paymentId}, PaymentNumber: {payment.PaymentNumber}", 
                    "Service");

                return paymentId;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error processing payment for claim ID: {payment.ClaimId}", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Service Layer Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return -1;
            }
        }

        public void ClearPayment(int paymentId, string modifiedBy)
        {
            LoggingHelper.LogInfo($"Clearing payment with ID: {paymentId}", "Service");

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
                LoggingHelper.LogInfo($"Payment with ID: {paymentId} cleared successfully", "Service");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error clearing payment with ID: {paymentId}", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Service Layer Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }
            }
        }

        private string GeneratePaymentNumber()
        {
            return $"PAY-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }

        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }
    }
}
