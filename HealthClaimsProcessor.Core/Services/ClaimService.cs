using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly ILogger<ClaimService> _logger;

        public ClaimService(IClaimRepository claimRepository, ILogger<ClaimService> logger)
        {
            _claimRepository = claimRepository;
            _logger = logger;
        }

        public List<Claim> GetAllClaims()
        {
            _logger.LogInformation("Getting all claims");

            try
            {
                return _claimRepository.GetAllClaims();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all claims");
                throw;
            }
        }

        public Claim GetClaimById(int claimId)
        {
            _logger.LogInformation("Getting claim by ID: {ClaimId}", claimId);

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
                _logger.LogError(ex, "Error retrieving claim with ID: {ClaimId}", claimId);
                throw;
            }
        }

        public List<Claim> GetClaimsByPatientId(int patientId)
        {
            _logger.LogInformation("Getting claims for patient ID: {PatientId}", patientId);

            try
            {
                return _claimRepository.GetClaimsByPatientId(patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving claims for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public int SubmitClaim(Claim claim)
        {
            _logger.LogInformation("Submitting new claim");

            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(claim, new ValidationContext(claim), validationResults, true))
                {
                    var messages = validationResults.Select(r => r.ErrorMessage).ToList();
                    string errorMessage = "Claim validation failed: " + string.Join("; ", messages);
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

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

                if (claim.LineItems != null)
                {
                    foreach (var lineItem in claim.LineItems)
                    {
                        lineItem.ClaimId = claimId;
                        _claimRepository.InsertClaimLineItem(lineItem);
                    }
                }

                _logger.LogInformation("Claim submitted successfully with ID: {ClaimId}, ClaimNumber: {ClaimNumber}", claimId, claim.ClaimNumber);
                return claimId;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting claim");
                throw;
            }
        }

        public void AdjudicateClaim(int claimId, ClaimStatus status, string processedBy, string adjudicationNotes)
        {
            _logger.LogInformation("Adjudicating claim ID: {ClaimId} with status: {Status}", claimId, status);

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
                _logger.LogInformation("Claim ID: {ClaimId} adjudicated successfully with status: {Status}", claimId, status);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjudicating claim ID: {ClaimId}", claimId);
                throw;
            }
        }
    }
}
