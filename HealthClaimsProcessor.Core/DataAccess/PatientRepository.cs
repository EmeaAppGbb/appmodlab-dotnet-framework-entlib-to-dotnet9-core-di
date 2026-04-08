using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class PatientRepository
    {
        private readonly Database _database;

        public PatientRepository()
        {
            _database = DatabaseFactory.GetDatabase();
        }

        public List<Patient> GetAllPatients()
        {
            LoggingHelper.LogInfo("Retrieving all patients", "DataAccess");

            var patients = new List<Patient>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetAllPatients"))
                using (IDataReader reader = _database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        patients.Add(MapPatient(reader));
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {patients.Count} patients", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all patients", ex, "DataAccess");
                throw;
            }

            return patients;
        }

        public Patient GetPatientById(int patientId)
        {
            LoggingHelper.LogInfo($"Retrieving patient with ID: {patientId}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetPatientById"))
                {
                    _database.AddInParameter(command, "@PatientId", DbType.Int32, patientId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            var patient = MapPatient(reader);
                            LoggingHelper.LogInfo($"Found patient: {patient.FirstName} {patient.LastName}", "DataAccess");
                            return patient;
                        }
                    }
                }

                LoggingHelper.LogWarning($"Patient with ID {patientId} not found", "DataAccess");
                return null;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving patient with ID: {patientId}", ex, "DataAccess");
                throw;
            }
        }

        public int InsertPatient(Patient patient)
        {
            LoggingHelper.LogInfo($"Inserting new patient: {patient.FirstName} {patient.LastName}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_InsertPatient"))
                {
                    _database.AddOutParameter(command, "@PatientId", DbType.Int32, 4);
                    _database.AddInParameter(command, "@FirstName", DbType.String, patient.FirstName);
                    _database.AddInParameter(command, "@LastName", DbType.String, patient.LastName);
                    _database.AddInParameter(command, "@DateOfBirth", DbType.DateTime, patient.DateOfBirth);
                    _database.AddInParameter(command, "@SSN", DbType.String, patient.SSN);
                    _database.AddInParameter(command, "@Address", DbType.String, patient.Address);
                    _database.AddInParameter(command, "@City", DbType.String, patient.City);
                    _database.AddInParameter(command, "@State", DbType.String, patient.State);
                    _database.AddInParameter(command, "@ZipCode", DbType.String, patient.ZipCode);
                    _database.AddInParameter(command, "@Phone", DbType.String, patient.Phone);
                    _database.AddInParameter(command, "@Email", DbType.String,
                        (object)patient.Email ?? DBNull.Value);
                    _database.AddInParameter(command, "@InsurancePolicyNumber", DbType.String, patient.InsurancePolicyNumber);
                    _database.AddInParameter(command, "@InsuranceGroupNumber", DbType.String, patient.InsuranceGroupNumber);
                    _database.AddInParameter(command, "@InsuranceEffectiveDate", DbType.DateTime, patient.InsuranceEffectiveDate);
                    _database.AddInParameter(command, "@InsuranceTerminationDate", DbType.DateTime,
                        patient.InsuranceTerminationDate.HasValue ? (object)patient.InsuranceTerminationDate.Value : DBNull.Value);
                    _database.AddInParameter(command, "@IsActive", DbType.Boolean, patient.IsActive);

                    _database.ExecuteNonQuery(command);

                    int newId = (int)_database.GetParameterValue(command, "@PatientId");
                    patient.PatientId = newId;

                    LoggingHelper.LogInfo($"Inserted patient with ID: {newId}", "DataAccess");
                    return newId;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error inserting patient: {patient.FirstName} {patient.LastName}", ex, "DataAccess");
                throw;
            }
        }

        public void UpdatePatient(Patient patient)
        {
            LoggingHelper.LogInfo($"Updating patient with ID: {patient.PatientId}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_UpdatePatient"))
                {
                    _database.AddInParameter(command, "@PatientId", DbType.Int32, patient.PatientId);
                    _database.AddInParameter(command, "@FirstName", DbType.String, patient.FirstName);
                    _database.AddInParameter(command, "@LastName", DbType.String, patient.LastName);
                    _database.AddInParameter(command, "@DateOfBirth", DbType.DateTime, patient.DateOfBirth);
                    _database.AddInParameter(command, "@SSN", DbType.String, patient.SSN);
                    _database.AddInParameter(command, "@Address", DbType.String, patient.Address);
                    _database.AddInParameter(command, "@City", DbType.String, patient.City);
                    _database.AddInParameter(command, "@State", DbType.String, patient.State);
                    _database.AddInParameter(command, "@ZipCode", DbType.String, patient.ZipCode);
                    _database.AddInParameter(command, "@Phone", DbType.String, patient.Phone);
                    _database.AddInParameter(command, "@Email", DbType.String,
                        (object)patient.Email ?? DBNull.Value);
                    _database.AddInParameter(command, "@InsurancePolicyNumber", DbType.String, patient.InsurancePolicyNumber);
                    _database.AddInParameter(command, "@InsuranceGroupNumber", DbType.String, patient.InsuranceGroupNumber);
                    _database.AddInParameter(command, "@InsuranceEffectiveDate", DbType.DateTime, patient.InsuranceEffectiveDate);
                    _database.AddInParameter(command, "@InsuranceTerminationDate", DbType.DateTime,
                        patient.InsuranceTerminationDate.HasValue ? (object)patient.InsuranceTerminationDate.Value : DBNull.Value);
                    _database.AddInParameter(command, "@IsActive", DbType.Boolean, patient.IsActive);

                    _database.ExecuteNonQuery(command);
                }

                LoggingHelper.LogInfo($"Updated patient with ID: {patient.PatientId}", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error updating patient with ID: {patient.PatientId}", ex, "DataAccess");
                throw;
            }
        }

        private Patient MapPatient(IDataReader reader)
        {
            return new Patient
            {
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                SSN = reader.GetString(reader.GetOrdinal("SSN")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                City = reader.GetString(reader.GetOrdinal("City")),
                State = reader.GetString(reader.GetOrdinal("State")),
                ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                InsurancePolicyNumber = reader.GetString(reader.GetOrdinal("InsurancePolicyNumber")),
                InsuranceGroupNumber = reader.GetString(reader.GetOrdinal("InsuranceGroupNumber")),
                InsuranceEffectiveDate = reader.GetDateTime(reader.GetOrdinal("InsuranceEffectiveDate")),
                InsuranceTerminationDate = reader.IsDBNull(reader.GetOrdinal("InsuranceTerminationDate"))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal("InsuranceTerminationDate")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
    }
}
