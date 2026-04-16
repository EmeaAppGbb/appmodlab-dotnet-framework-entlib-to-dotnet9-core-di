using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Data;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly HealthClaimsDbContext _context;
        private readonly ILogger<ProviderRepository> _logger;

        public ProviderRepository(HealthClaimsDbContext context, ILogger<ProviderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Provider> GetAllProviders()
        {
            _logger.LogInformation("Retrieving all providers");

            try
            {
                var providers = _context.Providers
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.Name)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} providers", providers.Count);
                return providers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all providers");
                throw;
            }
        }

        public Provider GetProviderById(int providerId)
        {
            _logger.LogInformation("Retrieving provider with ID: {ProviderId}", providerId);

            try
            {
                var provider = _context.Providers.FirstOrDefault(p => p.ProviderId == providerId);

                if (provider != null)
                {
                    _logger.LogInformation("Found provider: {Name}", provider.Name);
                }
                else
                {
                    _logger.LogWarning("Provider with ID {ProviderId} not found", providerId);
                }

                return provider;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving provider with ID: {ProviderId}", providerId);
                throw;
            }
        }

        public int InsertProvider(Provider provider)
        {
            _logger.LogInformation("Inserting new provider: {Name}", provider.Name);

            try
            {
                _context.Providers.Add(provider);
                _context.SaveChanges();

                _logger.LogInformation("Inserted provider with ID: {ProviderId}", provider.ProviderId);
                return provider.ProviderId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting provider: {Name}", provider.Name);
                throw;
            }
        }
    }
}
