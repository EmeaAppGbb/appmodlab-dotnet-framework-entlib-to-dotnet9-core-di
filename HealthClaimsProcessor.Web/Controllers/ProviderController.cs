using System;
using System.Web.Mvc;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;
using HealthClaimsProcessor.Core.Services;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class ProviderController : Controller
    {
        private readonly ProviderService _providerService;

        public ProviderController(ProviderService providerService)
        {
            _providerService = providerService;
        }

        public ActionResult Index()
        {
            LoggingHelper.LogInfo("Listing all providers", "Controller");

            try
            {
                var providers = _providerService.GetAllProviders();
                return View(providers);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error listing providers", ex, "Controller");
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            LoggingHelper.LogInfo($"Viewing provider details for ID: {id}", "Controller");

            try
            {
                var provider = _providerService.GetProviderById(id);
                if (provider == null)
                {
                    return HttpNotFound();
                }
                return View(provider);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error viewing provider details for ID: {id}", ex, "Controller");
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoggingHelper.LogInfo("Displaying provider create form", "Controller");
            return View(new Provider());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provider provider)
        {
            LoggingHelper.LogInfo("Creating new provider", "Controller");

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(provider);
                }

                int newId = _providerService.CreateProvider(provider);
                LoggingHelper.LogInfo($"Provider created with ID: {newId}", "Controller");
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error creating provider", ex, "Controller");
                ModelState.AddModelError("", ex.Message);
                return View(provider);
            }
        }
    }
}
