using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IPatientRepository patientRepository, ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public List<Patient> GetAllPatients()
        {
            _logger.LogInformation("Getting all patients");

            try
            {
                return _patientRepository.GetAllPatients();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public Patient GetPatientById(int patientId)
        {
            _logger.LogInformation("Getting patient by ID: {PatientId}", patientId);

            try
            {
                return _patientRepository.GetPatientById(patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", patientId);
                throw;
            }
        }

        public int CreatePatient(Patient patient)
        {
            _logger.LogInformation("Creating new patient");

            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(patient, new ValidationContext(patient), validationResults, true))
                {
                    var messages = validationResults.Select(r => r.ErrorMessage).ToList();
                    string errorMessage = "Patient validation failed: " + string.Join("; ", messages);
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                patient.CreatedDate = DateTime.Now;
                patient.IsActive = true;

                int newId = _patientRepository.InsertPatient(patient);
                _logger.LogInformation("Patient created successfully with ID: {PatientId}", newId);
                return newId;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient");
                throw;
            }
        }

        public void UpdatePatient(Patient patient)
        {
            _logger.LogInformation("Updating patient with ID: {PatientId}", patient.PatientId);

            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(patient, new ValidationContext(patient), validationResults, true))
                {
                    var messages = validationResults.Select(r => r.ErrorMessage).ToList();
                    string errorMessage = "Patient validation failed: " + string.Join("; ", messages);
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                patient.ModifiedDate = DateTime.Now;

                _patientRepository.UpdatePatient(patient);
                _logger.LogInformation("Patient updated successfully with ID: {PatientId}", patient.PatientId);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID: {PatientId}", patient.PatientId);
                throw;
            }
        }
    }
}
