namespace HealthClaimsProcessor.Core.Models
{
    public enum ClaimStatus
    {
        Draft = 0,
        Submitted = 1,
        UnderReview = 2,
        PendingInformation = 3,
        Approved = 4,
        PartiallyApproved = 5,
        Denied = 6,
        Paid = 7,
        Cancelled = 8
    }
}
