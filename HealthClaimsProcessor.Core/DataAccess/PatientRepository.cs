using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Data;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthClaimsDbContext _context;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(HealthClaimsDbContext context, ILogger<PatientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Patient> GetAllPatients()
        {
            _logger.LogInformation("Retrieving all patients");

            try
            {
                var patients = _context.Patients
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} patients", patients.Count);
                return patients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public Patient GetPatientById(int patientId)
        {
            _logger.LogInformation("Retrieving patient with ID: {PatientId}", patientId);

            try
            {
                var patient = _context.Patients.FirstOrDefault(p => p.PatientId == patientId);

                if (patient != null)
                {
                    _logger.LogInformation("Found patient: {FirstName} {LastName}", patient.FirstName, patient.LastName);
                }
                else
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", patientId);
                }

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", patientId);
                throw;
            }
        }

        public int InsertPatient(Patient patient)
        {
            _logger.LogInformation("Inserting new patient: {FirstName} {LastName}", patient.FirstName, patient.LastName);

            try
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();

                _logger.LogInformation("Inserted patient with ID: {PatientId}", patient.PatientId);
                return patient.PatientId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting patient: {FirstName} {LastName}", patient.FirstName, patient.LastName);
                throw;
            }
        }

        public void UpdatePatient(Patient patient)
        {
            _logger.LogInformation("Updating patient with ID: {PatientId}", patient.PatientId);

            try
            {
                var existing = _context.Patients.Find(patient.PatientId);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(patient);
                    existing.ModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                }

                _logger.LogInformation("Updated patient with ID: {PatientId}", patient.PatientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID: {PatientId}", patient.PatientId);
                throw;
            }
        }
    }
}
