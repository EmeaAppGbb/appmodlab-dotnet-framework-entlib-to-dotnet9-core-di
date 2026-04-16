using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Listing all patients");

            try
            {
                var patients = _patientService.GetAllPatients();
                return View(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing patients");
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Viewing patient details for ID: {PatientId}", id);

            try
            {
                var patient = _patientService.GetPatientById(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing patient details for ID: {PatientId}", id);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Displaying patient create form");
            return View(new Patient());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            _logger.LogInformation("Creating new patient");

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patient);
                }

                int newId = _patientService.CreatePatient(patient);
                _logger.LogInformation("Patient created with ID: {PatientId}", newId);
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient");
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Displaying edit form for patient ID: {PatientId}", id);

            try
            {
                var patient = _patientService.GetPatientById(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form for patient ID: {PatientId}", id);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient patient)
        {
            _logger.LogInformation("Updating patient ID: {PatientId}", patient.PatientId);

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patient);
                }

                _patientService.UpdatePatient(patient);
                _logger.LogInformation("Patient updated with ID: {PatientId}", patient.PatientId);
                return RedirectToAction("Details", new { id = patient.PatientId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient ID: {PatientId}", patient.PatientId);
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }
    }
}
