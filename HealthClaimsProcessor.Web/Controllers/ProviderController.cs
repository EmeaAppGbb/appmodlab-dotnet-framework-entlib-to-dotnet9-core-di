using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<ProviderController> _logger;

        public ProviderController(IProviderService providerService, ILogger<ProviderController> logger)
        {
            _providerService = providerService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Listing all providers");

            try
            {
                var providers = _providerService.GetAllProviders();
                return View(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing providers");
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Viewing provider details for ID: {ProviderId}", id);

            try
            {
                var provider = _providerService.GetProviderById(id);
                if (provider == null)
                {
                    return NotFound();
                }
                return View(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing provider details for ID: {ProviderId}", id);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Displaying provider create form");
            return View(new Provider());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Provider provider)
        {
            _logger.LogInformation("Creating new provider");

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(provider);
                }

                int newId = _providerService.CreateProvider(provider);
                _logger.LogInformation("Provider created with ID: {ProviderId}", newId);
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating provider");
                ModelState.AddModelError("", ex.Message);
                return View(provider);
            }
        }
    }
}
