using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Data;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly HealthClaimsDbContext _context;
        private readonly ILogger<ClaimRepository> _logger;

        public ClaimRepository(HealthClaimsDbContext context, ILogger<ClaimRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Claim> GetAllClaims()
        {
            _logger.LogInformation("Retrieving all claims");

            try
            {
                var claims = _context.Claims
                    .Include(c => c.Patient)
                    .Include(c => c.Provider)
                    .OrderByDescending(c => c.SubmittedDate)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} claims", claims.Count);
                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all claims");
                throw;
            }
        }

        public Claim GetClaimById(int claimId)
        {
            _logger.LogInformation("Retrieving claim with ID: {ClaimId}", claimId);

            try
            {
                var claim = _context.Claims
                    .Include(c => c.Patient)
                    .Include(c => c.Provider)
                    .FirstOrDefault(c => c.ClaimId == claimId);

                if (claim != null)
                {
                    _logger.LogInformation("Found claim: {ClaimNumber}", claim.ClaimNumber);
                }
                else
                {
                    _logger.LogWarning("Claim with ID {ClaimId} not found", claimId);
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
            _logger.LogInformation("Retrieving claims for patient ID: {PatientId}", patientId);

            try
            {
                var claims = _context.Claims
                    .Where(c => c.PatientId == patientId)
                    .OrderByDescending(c => c.SubmittedDate)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} claims for patient ID: {PatientId}", claims.Count, patientId);
                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving claims for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public int InsertClaim(Claim claim)
        {
            _logger.LogInformation("Inserting new claim: {ClaimNumber}", claim.ClaimNumber);

            try
            {
                _context.Claims.Add(claim);
                _context.SaveChanges();

                _logger.LogInformation("Inserted claim with ID: {ClaimId}", claim.ClaimId);
                return claim.ClaimId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting claim: {ClaimNumber}", claim.ClaimNumber);
                throw;
            }
        }

        public void UpdateClaimStatus(int claimId, ClaimStatus status, string processedBy, string adjudicationNotes)
        {
            _logger.LogInformation("Updating claim status for claim ID: {ClaimId} to {Status}", claimId, status);

            try
            {
                var claim = _context.Claims.Find(claimId);
                if (claim != null)
                {
                    claim.Status = status;
                    claim.ProcessedBy = processedBy;
                    claim.ProcessedDate = DateTime.Now;
                    claim.AdjudicationNotes = adjudicationNotes;
                    claim.ModifiedBy = processedBy;
                    claim.ModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                }

                _logger.LogInformation("Updated claim ID: {ClaimId} status to {Status}", claimId, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating claim status for claim ID: {ClaimId}", claimId);
                throw;
            }
        }

        public int InsertClaimLineItem(ClaimLineItem lineItem)
        {
            _logger.LogInformation("Inserting line item for claim ID: {ClaimId}, line number: {LineNumber}", lineItem.ClaimId, lineItem.LineNumber);

            try
            {
                _context.ClaimLineItems.Add(lineItem);
                _context.SaveChanges();

                _logger.LogInformation("Inserted line item with ID: {LineItemId} for claim ID: {ClaimId}", lineItem.LineItemId, lineItem.ClaimId);
                return lineItem.LineItemId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting line item for claim ID: {ClaimId}", lineItem.ClaimId);
                throw;
            }
        }

        public List<ClaimLineItem> GetClaimLineItems(int claimId)
        {
            _logger.LogInformation("Retrieving line items for claim ID: {ClaimId}", claimId);

            try
            {
                var lineItems = _context.ClaimLineItems
                    .Where(li => li.ClaimId == claimId)
                    .OrderBy(li => li.LineNumber)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} line items for claim ID: {ClaimId}", lineItems.Count, claimId);
                return lineItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving line items for claim ID: {ClaimId}", claimId);
                throw;
            }
        }
    }
}
