using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ILogger<ProviderService> _logger;

        public ProviderService(IProviderRepository providerRepository, ILogger<ProviderService> logger)
        {
            _providerRepository = providerRepository;
            _logger = logger;
        }

        public List<Provider> GetAllProviders()
        {
            _logger.LogInformation("Getting all providers");

            try
            {
                return _providerRepository.GetAllProviders();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all providers");
                throw;
            }
        }

        public Provider GetProviderById(int providerId)
        {
            _logger.LogInformation("Getting provider by ID: {ProviderId}", providerId);

            try
            {
                return _providerRepository.GetProviderById(providerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving provider with ID: {ProviderId}", providerId);
                throw;
            }
        }

        public int CreateProvider(Provider provider)
        {
            _logger.LogInformation("Creating new provider");

            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(provider, new ValidationContext(provider), validationResults, true))
                {
                    var messages = validationResults.Select(r => r.ErrorMessage).ToList();
                    string errorMessage = "Provider validation failed: " + string.Join("; ", messages);
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                provider.CreatedDate = DateTime.Now;
                provider.IsActive = true;

                int newId = _providerRepository.InsertProvider(provider);
                _logger.LogInformation("Provider created successfully with ID: {ProviderId}", newId);
                return newId;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating provider");
                throw;
            }
        }
    }
}
