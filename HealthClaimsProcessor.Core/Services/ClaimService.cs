using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using HealthClaimsProcessor.Core.DataAccess;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class ClaimService
    {
        private readonly ClaimRepository _claimRepository;
        private readonly ExceptionManager _exceptionManager;

        public ClaimService(ClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
            _exceptionManager = new ExceptionPolicyFactory(new SystemConfigurationSource()).CreateManager();
        }

        public List<Claim> GetAllClaims()
        {
            LoggingHelper.LogInfo("Getting all claims", "Service");

            try
            {
                return _claimRepository.GetAllClaims();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all claims", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return new List<Claim>();
            }
        }

        public Claim GetClaimById(int claimId)
        {
            LoggingHelper.LogInfo($"Getting claim by ID: {claimId}", "Service");

            try
            {
                var claim = _claimRepository.GetClaimById(claimId);

                if (claim != null)
                {
                    claim.LineItems = _claimRepository.GetClaimLineItems(claimId);
                }

                return claim;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving claim with ID: {claimId}", ex, "Service");

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

        public List<Claim> GetClaimsByPatientId(int patientId)
        {
            LoggingHelper.LogInfo($"Getting claims for patient ID: {patientId}", "Service");

            try
            {
                return _claimRepository.GetClaimsByPatientId(patientId);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving claims for patient ID: {patientId}", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return new List<Claim>();
            }
        }

        public int SubmitClaim(Claim claim)
        {
            LoggingHelper.LogInfo("Submitting new claim", "Service");

            try
            {
                var validator = ValidationFactory.CreateValidator<Claim>();
                var validationResults = validator.Validate(claim);
                if (!validationResults.IsValid)
                {
                    var messages = new List<string>();
                    foreach (var result in validationResults)
                    {
                        messages.Add(result.Message);
                    }
                    string errorMessage = "Claim validation failed: " + string.Join("; ", messages);
                    LoggingHelper.LogWarning(errorMessage, "Service");
                    throw new ValidationException(errorMessage);
                }

                // Calculate TotalChargeAmount from line items
                if (claim.LineItems != null && claim.LineItems.Any())
                {
                    foreach (var lineItem in claim.LineItems)
                    {
                        lineItem.TotalCharge = lineItem.Quantity * lineItem.UnitCharge;
                    }
                    claim.TotalChargeAmount = claim.LineItems.Sum(li => li.TotalCharge);
                }

                claim.CreatedDate = DateTime.Now;
                claim.SubmittedDate = DateTime.Now;
                claim.Status = ClaimStatus.Submitted;

                int claimId = _claimRepository.InsertClaim(claim);

                // Insert line items
                if (claim.LineItems != null)
                {
                    foreach (var lineItem in claim.LineItems)
                    {
                        lineItem.ClaimId = claimId;
                        _claimRepository.InsertClaimLineItem(lineItem);
                    }
                }

                LoggingHelper.LogInfo($"Claim submitted successfully with ID: {claimId}, ClaimNumber: {claim.ClaimNumber}", "Service");
                return claimId;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error submitting claim", ex, "Service");

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

        public void AdjudicateClaim(int claimId, ClaimStatus status, string processedBy, string adjudicationNotes)
        {
            LoggingHelper.LogInfo($"Adjudicating claim ID: {claimId} with status: {status}", "Service");

            try
            {
                var claim = _claimRepository.GetClaimById(claimId);
                if (claim == null)
                {
                    throw new InvalidOperationException($"Claim with ID {claimId} not found");
                }

                if (claim.Status != ClaimStatus.Submitted && claim.Status != ClaimStatus.UnderReview)
                {
                    throw new InvalidOperationException(
                        $"Claim with ID {claimId} cannot be adjudicated. Current status: {claim.Status}");
                }

                _claimRepository.UpdateClaimStatus(claimId, status, processedBy, adjudicationNotes);
                LoggingHelper.LogInfo($"Claim ID: {claimId} adjudicated successfully with status: {status}", "Service");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error adjudicating claim ID: {claimId}", ex, "Service");

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

        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }
    }
}
