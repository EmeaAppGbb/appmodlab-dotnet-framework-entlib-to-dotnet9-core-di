using HealthClaimsProcessor.Core.Models;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HealthClaimsProcessor.Core.Validation
{
    public static class ClaimValidators
    {
        public static ValidationResults ValidateClaim(Claim claim)
        {
            Validator<Claim> validator = ValidationFactory.CreateValidator<Claim>();
            return validator.Validate(claim);
        }

        public static ValidationResults ValidateClaimLineItem(ClaimLineItem lineItem)
        {
            Validator<ClaimLineItem> validator = ValidationFactory.CreateValidator<ClaimLineItem>();
            return validator.Validate(lineItem);
        }

        public static ValidationResults ValidatePatient(Patient patient)
        {
            Validator<Patient> validator = ValidationFactory.CreateValidator<Patient>();
            return validator.Validate(patient);
        }

        public static ValidationResults ValidateProvider(Provider provider)
        {
            Validator<Provider> validator = ValidationFactory.CreateValidator<Provider>();
            return validator.Validate(provider);
        }

        public static ValidationResults ValidatePayment(Payment payment)
        {
            Validator<Payment> validator = ValidationFactory.CreateValidator<Payment>();
            return validator.Validate(payment);
        }
    }
}
