using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IProviderService
    {
        List<Provider> GetAllProviders();
        Provider GetProviderById(int providerId);
        int CreateProvider(Provider provider);
    }
}
