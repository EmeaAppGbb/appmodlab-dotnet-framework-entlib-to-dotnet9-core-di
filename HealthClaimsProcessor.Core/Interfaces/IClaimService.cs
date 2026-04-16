using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IClaimService
    {
        List<Claim> GetAllClaims();
        Claim GetClaimById(int claimId);
        List<Claim> GetClaimsByPatientId(int patientId);
        int SubmitClaim(Claim claim);
        void AdjudicateClaim(int claimId, ClaimStatus status, string processedBy, string adjudicationNotes);
    }
}
