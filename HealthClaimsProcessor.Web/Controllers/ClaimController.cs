using System;
using System.Web.Mvc;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;
using HealthClaimsProcessor.Core.Services;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly PatientService _patientService;
        private readonly ProviderService _providerService;

        public ClaimController(ClaimService claimService, PatientService patientService, ProviderService providerService)
        {
            _claimService = claimService;
            _patientService = patientService;
            _providerService = providerService;
        }

        public ActionResult Index()
        {
            LoggingHelper.LogInfo("Listing all claims", "Controller");

            try
            {
                var claims = _claimService.GetAllClaims();
                return View(claims);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error listing claims", ex, "Controller");
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            LoggingHelper.LogInfo($"Viewing claim details for ID: {id}", "Controller");

            try
            {
                var claim = _claimService.GetClaimById(id);
                if (claim == null)
                {
                    return HttpNotFound();
                }
                return View(claim);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error viewing claim details for ID: {id}", ex, "Controller");
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoggingHelper.LogInfo("Displaying claim create form", "Controller");

            try
            {
                PopulateDropdowns();
                return View(new Claim());
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error loading claim create form", ex, "Controller");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Claim claim)
        {
            LoggingHelper.LogInfo("Submitting new claim", "Controller");

            try
            {
                if (!ModelState.IsValid)
                {
                    PopulateDropdowns();
                    return View(claim);
                }

                int newId = _claimService.SubmitClaim(claim);
                LoggingHelper.LogInfo($"Claim submitted with ID: {newId}", "Controller");
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error submitting claim", ex, "Controller");
                ModelState.AddModelError("", ex.Message);
                PopulateDropdowns();
                return View(claim);
            }
        }

        [HttpGet]
        public ActionResult Adjudicate(int id)
        {
            LoggingHelper.LogInfo($"Displaying adjudication form for claim ID: {id}", "Controller");

            try
            {
                var claim = _claimService.GetClaimById(id);
                if (claim == null)
                {
                    return HttpNotFound();
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
                LoggingHelper.LogError($"Error loading adjudication form for claim ID: {id}", ex, "Controller");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adjudicate(int id, ClaimStatus status, string notes)
        {
            LoggingHelper.LogInfo($"Adjudicating claim ID: {id} with status: {status}", "Controller");

            try
            {
                _claimService.AdjudicateClaim(id, status, User.Identity.Name, notes);
                LoggingHelper.LogInfo($"Claim ID: {id} adjudicated successfully", "Controller");
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error adjudicating claim ID: {id}", ex, "Controller");
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
