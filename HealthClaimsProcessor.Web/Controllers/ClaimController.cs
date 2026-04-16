using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class ClaimController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly IPatientService _patientService;
        private readonly IProviderService _providerService;
        private readonly ILogger<ClaimController> _logger;

        public ClaimController(IClaimService claimService, IPatientService patientService,
            IProviderService providerService, ILogger<ClaimController> logger)
        {
            _claimService = claimService;
            _patientService = patientService;
            _providerService = providerService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Listing all claims");

            try
            {
                var claims = _claimService.GetAllClaims();
                return View(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing claims");
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Viewing claim details for ID: {ClaimId}", id);

            try
            {
                var claim = _claimService.GetClaimById(id);
                if (claim == null)
                {
                    return NotFound();
                }
                return View(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing claim details for ID: {ClaimId}", id);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Displaying claim create form");

            try
            {
                PopulateDropdowns();
                return View(new Claim());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading claim create form");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Claim claim)
        {
            _logger.LogInformation("Submitting new claim");

            try
            {
                if (!ModelState.IsValid)
                {
                    PopulateDropdowns();
                    return View(claim);
                }

                int newId = _claimService.SubmitClaim(claim);
                _logger.LogInformation("Claim submitted with ID: {ClaimId}", newId);
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting claim");
                ModelState.AddModelError("", ex.Message);
                PopulateDropdowns();
                return View(claim);
            }
        }

        [HttpGet]
        public IActionResult Adjudicate(int id)
        {
            _logger.LogInformation("Displaying adjudication form for claim ID: {ClaimId}", id);

            try
            {
                var claim = _claimService.GetClaimById(id);
                if (claim == null)
                {
                    return NotFound();
                }

                ViewBag.StatusList = new SelectList(new[]
                {
                    ClaimStatus.Approved,
                    ClaimStatus.PartiallyApproved,
                    ClaimStatus.Denied
                });

                return View(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading adjudication form for claim ID: {ClaimId}", id);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adjudicate(int id, ClaimStatus status, string notes)
        {
            _logger.LogInformation("Adjudicating claim ID: {ClaimId} with status: {Status}", id, status);

            try
            {
                _claimService.AdjudicateClaim(id, status, User.Identity?.Name ?? "System", notes);
                _logger.LogInformation("Claim ID: {ClaimId} adjudicated successfully", id);
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjudicating claim ID: {ClaimId}", id);
                ModelState.AddModelError("", ex.Message);

                var claim = _claimService.GetClaimById(id);
                ViewBag.StatusList = new SelectList(new[]
                {
                    ClaimStatus.Approved,
                    ClaimStatus.PartiallyApproved,
                    ClaimStatus.Denied
                });

                return View(claim);
            }
        }

        private void PopulateDropdowns()
        {
            var patients = _patientService.GetAllPatients();
            var providers = _providerService.GetAllProviders();

            ViewBag.PatientId = new SelectList(patients, "PatientId", "LastName");
            ViewBag.ProviderId = new SelectList(providers, "ProviderId", "Name");
        }
    }
}
