using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IClaimRepository
    {
        List<Claim> GetAllClaims();
        Claim GetClaimById(int claimId);
        List<Claim> GetClaimsByPatientId(int patientId);
        int InsertClaim(Claim claim);
        void UpdateClaimStatus(int claimId, ClaimStatus status, string processedBy, string adjudicationNotes);
        int InsertClaimLineItem(ClaimLineItem lineItem);
        List<ClaimLineItem> GetClaimLineItems(int claimId);
    }
}
