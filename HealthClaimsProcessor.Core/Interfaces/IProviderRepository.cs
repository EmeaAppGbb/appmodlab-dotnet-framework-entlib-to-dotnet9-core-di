using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IProviderRepository
    {
        List<Provider> GetAllProviders();
        Provider GetProviderById(int providerId);
        int InsertProvider(Provider provider);
    }
}
