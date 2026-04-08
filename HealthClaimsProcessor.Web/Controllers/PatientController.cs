using System;
using System.Web.Mvc;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;
using HealthClaimsProcessor.Core.Services;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        public ActionResult Index()
        {
            LoggingHelper.LogInfo("Listing all patients", "Controller");

            try
            {
                var patients = _patientService.GetAllPatients();
                return View(patients);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error listing patients", ex, "Controller");
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            LoggingHelper.LogInfo($"Viewing patient details for ID: {id}", "Controller");

            try
            {
                var patient = _patientService.GetPatientById(id);
                if (patient == null)
                {
                    return HttpNotFound();
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error viewing patient details for ID: {id}", ex, "Controller");
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoggingHelper.LogInfo("Displaying patient create form", "Controller");
            return View(new Patient());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            LoggingHelper.LogInfo("Creating new patient", "Controller");

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patient);
                }

                int newId = _patientService.CreatePatient(patient);
                LoggingHelper.LogInfo($"Patient created with ID: {newId}", "Controller");
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error creating patient", ex, "Controller");
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            LoggingHelper.LogInfo($"Displaying edit form for patient ID: {id}", "Controller");

            try
            {
                var patient = _patientService.GetPatientById(id);
                if (patient == null)
                {
                    return HttpNotFound();
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error loading edit form for patient ID: {id}", ex, "Controller");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            LoggingHelper.LogInfo($"Updating patient ID: {patient.PatientId}", "Controller");

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patient);
                }

                _patientService.UpdatePatient(patient);
                LoggingHelper.LogInfo($"Patient updated with ID: {patient.PatientId}", "Controller");
                return RedirectToAction("Details", new { id = patient.PatientId });
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error updating patient ID: {patient.PatientId}", ex, "Controller");
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }
    }
}
