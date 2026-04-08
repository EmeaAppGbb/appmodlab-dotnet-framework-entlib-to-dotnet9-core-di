using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using HealthClaimsProcessor.Core.DataAccess;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;
        private readonly ExceptionManager _exceptionManager;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
            _exceptionManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
        }

        public List<Patient> GetAllPatients()
        {
            LoggingHelper.LogInfo("Getting all patients", "Service");

            try
            {
                return _patientRepository.GetAllPatients();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all patients", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return new List<Patient>();
            }
        }

        public Patient GetPatientById(int patientId)
        {
            LoggingHelper.LogInfo($"Getting patient by ID: {patientId}", "Service");

            try
            {
                return _patientRepository.GetPatientById(patientId);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving patient with ID: {patientId}", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return null;
            }
        }

        public int CreatePatient(Patient patient)
        {
            LoggingHelper.LogInfo("Creating new patient", "Service");

            try
            {
                var validationResults = Validation.Validate(patient);
                if (!validationResults.IsValid)
                {
                    var messages = new List<string>();
                    foreach (var result in validationResults)
                    {
                        messages.Add(result.Message);
                    }
                    string errorMessage = "Patient validation failed: " + string.Join("; ", messages);
                    LoggingHelper.LogWarning(errorMessage, "Service");
                    throw new ValidationException(errorMessage);
                }

                patient.CreatedDate = DateTime.Now;
                patient.IsActive = true;

                int newId = _patientRepository.InsertPatient(patient);
                LoggingHelper.LogInfo($"Patient created successfully with ID: {newId}", "Service");
                return newId;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error creating patient", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Service Layer Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return -1;
            }
        }

        public void UpdatePatient(Patient patient)
        {
            LoggingHelper.LogInfo($"Updating patient with ID: {patient.PatientId}", "Service");

            try
            {
                var validationResults = Validation.Validate(patient);
                if (!validationResults.IsValid)
                {
                    var messages = new List<string>();
                    foreach (var result in validationResults)
                    {
                        messages.Add(result.Message);
                    }
                    string errorMessage = "Patient validation failed: " + string.Join("; ", messages);
                    LoggingHelper.LogWarning(errorMessage, "Service");
                    throw new ValidationException(errorMessage);
                }

                patient.ModifiedDate = DateTime.Now;

                _patientRepository.UpdatePatient(patient);
                LoggingHelper.LogInfo($"Patient updated successfully with ID: {patient.PatientId}", "Service");
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error updating patient with ID: {patient.PatientId}", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Service Layer Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }
            }
        }

        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }
    }
}
