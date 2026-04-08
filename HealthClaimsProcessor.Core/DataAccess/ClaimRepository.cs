using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class ClaimRepository
    {
        private readonly Database _database;

        public ClaimRepository()
        {
            _database = DatabaseFactory.GetDatabase();
        }

        public List<Claim> GetAllClaims()
        {
            LoggingHelper.LogInfo("Retrieving all claims", "DataAccess");

            var claims = new List<Claim>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetAllClaims"))
                using (IDataReader reader = _database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        claims.Add(MapClaim(reader));
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {claims.Count} claims", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all claims", ex, "DataAccess");
                throw;
            }

            return claims;
        }

        public Claim GetClaimById(int claimId)
        {
            LoggingHelper.LogInfo($"Retrieving claim with ID: {claimId}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetClaimById"))
                {
                    _database.AddInParameter(command, "@ClaimId", DbType.Int32, claimId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            var claim = MapClaim(reader);
                            LoggingHelper.LogInfo($"Found claim: {claim.ClaimNumber}", "DataAccess");
                            return claim;
                        }
                    }
                }

                LoggingHelper.LogWarning($"Claim with ID {claimId} not found", "DataAccess");
                return null;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving claim with ID: {claimId}", ex, "DataAccess");
                throw;
            }
        }

        public List<Claim> GetClaimsByPatientId(int patientId)
        {
            LoggingHelper.LogInfo($"Retrieving claims for patient ID: {patientId}", "DataAccess");

            var claims = new List<Claim>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetClaimsByPatientId"))
                {
                    _database.AddInParameter(command, "@PatientId", DbType.Int32, patientId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            claims.Add(MapClaim(reader));
                        }
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {claims.Count} claims for patient ID: {patientId}", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving claims for patient ID: {patientId}", ex, "DataAccess");
                throw;
            }

            return claims;
        }

        public int InsertClaim(Claim claim)
        {
            LoggingHelper.LogInfo($"Inserting new claim: {claim.ClaimNumber}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_InsertClaim"))
                {
                    _database.AddOutParameter(command, "@ClaimId", DbType.Int32, 4);
                    _database.AddInParameter(command, "@ClaimNumber", DbType.String, claim.ClaimNumber);
                    _database.AddInParameter(command, "@PatientId", DbType.Int32, claim.PatientId);
                    _database.AddInParameter(command, "@ProviderId", DbType.Int32, claim.ProviderId);
                    _database.AddInParameter(command, "@ServiceDateFrom", DbType.DateTime, claim.ServiceDateFrom);
                    _database.AddInParameter(command, "@ServiceDateTo", DbType.DateTime, claim.ServiceDateTo);
                    _database.AddInParameter(command, "@SubmittedDate", DbType.DateTime, claim.SubmittedDate);
                    _database.AddInParameter(command, "@Status", DbType.Int32, (int)claim.Status);
                    _database.AddInParameter(command, "@PlaceOfService", DbType.String, claim.PlaceOfService);
                    _database.AddInParameter(command, "@DiagnosisPointer", DbType.String,
                        (object)claim.DiagnosisPointer ?? DBNull.Value);
                    _database.AddInParameter(command, "@TotalChargeAmount", DbType.Decimal, claim.TotalChargeAmount);
                    _database.AddInParameter(command, "@TotalApprovedAmount", DbType.Decimal, claim.TotalApprovedAmount);
                    _database.AddInParameter(command, "@TotalPatientResponsibility", DbType.Decimal, claim.TotalPatientResponsibility);
                    _database.AddInParameter(command, "@PriorAuthorizationNumber", DbType.String,
                        (object)claim.PriorAuthorizationNumber ?? DBNull.Value);
                    _database.AddInParameter(command, "@ReferringProviderNPI", DbType.String,
                        (object)claim.ReferringProviderNPI ?? DBNull.Value);
                    _database.AddInParameter(command, "@AdmissionDate", DbType.String,
                        (object)claim.AdmissionDate ?? DBNull.Value);
                    _database.AddInParameter(command, "@DischargeDate", DbType.String,
                        (object)claim.DischargeDate ?? DBNull.Value);
                    _database.AddInParameter(command, "@AccidentDate", DbType.String,
                        (object)claim.AccidentDate ?? DBNull.Value);
                    _database.AddInParameter(command, "@IsAccidentRelated", DbType.Boolean, claim.IsAccidentRelated);
                    _database.AddInParameter(command, "@IsEmploymentRelated", DbType.Boolean, claim.IsEmploymentRelated);
                    _database.AddInParameter(command, "@ProcessedBy", DbType.String,
                        (object)claim.ProcessedBy ?? DBNull.Value);
                    _database.AddInParameter(command, "@ProcessedDate", DbType.DateTime,
                        claim.ProcessedDate.HasValue ? (object)claim.ProcessedDate.Value : DBNull.Value);
                    _database.AddInParameter(command, "@AdjudicationNotes", DbType.String,
                        (object)claim.AdjudicationNotes ?? DBNull.Value);
                    _database.AddInParameter(command, "@CreatedBy", DbType.String,
                        (object)claim.CreatedBy ?? DBNull.Value);

                    _database.ExecuteNonQuery(command);

                    int newId = (int)_database.GetParameterValue(command, "@ClaimId");
                    claim.ClaimId = newId;

                    LoggingHelper.LogInfo($"Inserted claim with ID: {newId}", "DataAccess");
                    return newId;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error inserting claim: {claim.ClaimNumber}", ex, "DataAccess");
                throw;
            }
        }

        public void UpdateClaimStatus(int claimId, ClaimStatus status, string processedBy, string adjudicationNotes)
        {
            LoggingHelper.LogInfo($"Updating claim status for claim ID: {claimId} to {status}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_UpdateClaimStatus"))
                {
                    _database.AddInParameter(command, "@ClaimId", DbType.Int32, claimId);
                    _database.AddInParameter(command, "@Status", DbType.Int32, (int)status);
                    _database.AddInParameter(command, "@ProcessedBy", DbType.String,
                        (object)processedBy ?? DBNull.Value);
                    _database.AddInParameter(command, "@ProcessedDate", DbType.DateTime, DateTime.Now);
                    _database.AddInParameter(command, "@AdjudicationNotes", DbType.String,
                        (object)adjudicationNotes ?? DBNull.Value);
                    _database.AddInParameter(command, "@ModifiedBy", DbType.String,
                        (object)processedBy ?? DBNull.Value);

                    _database.ExecuteNonQuery(command);
                }

                LoggingHelper.LogInfo($"Updated claim ID: {claimId} status to {status}", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error updating claim status for claim ID: {claimId}", ex, "DataAccess");
                throw;
            }
        }

        public int InsertClaimLineItem(ClaimLineItem lineItem)
        {
            LoggingHelper.LogInfo($"Inserting line item for claim ID: {lineItem.ClaimId}, line number: {lineItem.LineNumber}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_InsertClaimLineItem"))
                {
                    _database.AddOutParameter(command, "@LineItemId", DbType.Int32, 4);
                    _database.AddInParameter(command, "@ClaimId", DbType.Int32, lineItem.ClaimId);
                    _database.AddInParameter(command, "@LineNumber", DbType.Int32, lineItem.LineNumber);
                    _database.AddInParameter(command, "@ServiceDate", DbType.DateTime, lineItem.ServiceDate);
                    _database.AddInParameter(command, "@CPTCode", DbType.String, lineItem.CPTCode);
                    _database.AddInParameter(command, "@CPTDescription", DbType.String,
                        (object)lineItem.CPTDescription ?? DBNull.Value);
                    _database.AddInParameter(command, "@ICD10Code", DbType.String, lineItem.ICD10Code);
                    _database.AddInParameter(command, "@ICD10Description", DbType.String,
                        (object)lineItem.ICD10Description ?? DBNull.Value);
                    _database.AddInParameter(command, "@Quantity", DbType.Int32, lineItem.Quantity);
                    _database.AddInParameter(command, "@UnitCharge", DbType.Decimal, lineItem.UnitCharge);
                    _database.AddInParameter(command, "@TotalCharge", DbType.Decimal, lineItem.TotalCharge);
                    _database.AddInParameter(command, "@ApprovedAmount", DbType.Decimal, lineItem.ApprovedAmount);
                    _database.AddInParameter(command, "@PatientResponsibility", DbType.Decimal, lineItem.PatientResponsibility);
                    _database.AddInParameter(command, "@DenialReason", DbType.String,
                        (object)lineItem.DenialReason ?? DBNull.Value);
                    _database.AddInParameter(command, "@Notes", DbType.String,
                        (object)lineItem.Notes ?? DBNull.Value);

                    _database.ExecuteNonQuery(command);

                    int newId = (int)_database.GetParameterValue(command, "@LineItemId");
                    lineItem.LineItemId = newId;

                    LoggingHelper.LogInfo($"Inserted line item with ID: {newId} for claim ID: {lineItem.ClaimId}", "DataAccess");
                    return newId;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error inserting line item for claim ID: {lineItem.ClaimId}", ex, "DataAccess");
                throw;
            }
        }

        public List<ClaimLineItem> GetClaimLineItems(int claimId)
        {
            LoggingHelper.LogInfo($"Retrieving line items for claim ID: {claimId}", "DataAccess");

            var lineItems = new List<ClaimLineItem>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetClaimLineItems"))
                {
                    _database.AddInParameter(command, "@ClaimId", DbType.Int32, claimId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            lineItems.Add(MapClaimLineItem(reader));
                        }
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {lineItems.Count} line items for claim ID: {claimId}", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving line items for claim ID: {claimId}", ex, "DataAccess");
                throw;
            }

            return lineItems;
        }

        private Claim MapClaim(IDataReader reader)
        {
            return new Claim
            {
                ClaimId = reader.GetInt32(reader.GetOrdinal("ClaimId")),
                ClaimNumber = reader.GetString(reader.GetOrdinal("ClaimNumber")),
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                ProviderId = reader.GetInt32(reader.GetOrdinal("ProviderId")),
                ServiceDateFrom = reader.GetDateTime(reader.GetOrdinal("ServiceDateFrom")),
                ServiceDateTo = reader.GetDateTime(reader.GetOrdinal("ServiceDateTo")),
                SubmittedDate = reader.GetDateTime(reader.GetOrdinal("SubmittedDate")),
                Status = (ClaimStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                PlaceOfService = reader.GetString(reader.GetOrdinal("PlaceOfService")),
                DiagnosisPointer = reader.IsDBNull(reader.GetOrdinal("DiagnosisPointer"))
                    ? null : reader.GetString(reader.GetOrdinal("DiagnosisPointer")),
                TotalChargeAmount = reader.GetDecimal(reader.GetOrdinal("TotalChargeAmount")),
                TotalApprovedAmount = reader.GetDecimal(reader.GetOrdinal("TotalApprovedAmount")),
                TotalPatientResponsibility = reader.GetDecimal(reader.GetOrdinal("TotalPatientResponsibility")),
                PriorAuthorizationNumber = reader.IsDBNull(reader.GetOrdinal("PriorAuthorizationNumber"))
                    ? null : reader.GetString(reader.GetOrdinal("PriorAuthorizationNumber")),
                ReferringProviderNPI = reader.IsDBNull(reader.GetOrdinal("ReferringProviderNPI"))
                    ? null : reader.GetString(reader.GetOrdinal("ReferringProviderNPI")),
                AdmissionDate = reader.IsDBNull(reader.GetOrdinal("AdmissionDate"))
                    ? null : reader.GetString(reader.GetOrdinal("AdmissionDate")),
                DischargeDate = reader.IsDBNull(reader.GetOrdinal("DischargeDate"))
                    ? null : reader.GetString(reader.GetOrdinal("DischargeDate")),
                AccidentDate = reader.IsDBNull(reader.GetOrdinal("AccidentDate"))
                    ? null : reader.GetString(reader.GetOrdinal("AccidentDate")),
                IsAccidentRelated = reader.GetBoolean(reader.GetOrdinal("IsAccidentRelated")),
                IsEmploymentRelated = reader.GetBoolean(reader.GetOrdinal("IsEmploymentRelated")),
                ProcessedBy = reader.IsDBNull(reader.GetOrdinal("ProcessedBy"))
                    ? null : reader.GetString(reader.GetOrdinal("ProcessedBy")),
                ProcessedDate = reader.IsDBNull(reader.GetOrdinal("ProcessedDate"))
                    ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ProcessedDate")),
                AdjudicationNotes = reader.IsDBNull(reader.GetOrdinal("AdjudicationNotes"))
                    ? null : reader.GetString(reader.GetOrdinal("AdjudicationNotes")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))
                    ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy"))
                    ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy"))
                    ? null : reader.GetString(reader.GetOrdinal("ModifiedBy"))
            };
        }

        private ClaimLineItem MapClaimLineItem(IDataReader reader)
        {
            return new ClaimLineItem
            {
                LineItemId = reader.GetInt32(reader.GetOrdinal("LineItemId")),
                ClaimId = reader.GetInt32(reader.GetOrdinal("ClaimId")),
                LineNumber = reader.GetInt32(reader.GetOrdinal("LineNumber")),
                ServiceDate = reader.GetDateTime(reader.GetOrdinal("ServiceDate")),
                CPTCode = reader.GetString(reader.GetOrdinal("CPTCode")),
                CPTDescription = reader.IsDBNull(reader.GetOrdinal("CPTDescription"))
                    ? null : reader.GetString(reader.GetOrdinal("CPTDescription")),
                ICD10Code = reader.GetString(reader.GetOrdinal("ICD10Code")),
                ICD10Description = reader.IsDBNull(reader.GetOrdinal("ICD10Description"))
                    ? null : reader.GetString(reader.GetOrdinal("ICD10Description")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                UnitCharge = reader.GetDecimal(reader.GetOrdinal("UnitCharge")),
                TotalCharge = reader.GetDecimal(reader.GetOrdinal("TotalCharge")),
                ApprovedAmount = reader.GetDecimal(reader.GetOrdinal("ApprovedAmount")),
                PatientResponsibility = reader.GetDecimal(reader.GetOrdinal("PatientResponsibility")),
                DenialReason = reader.IsDBNull(reader.GetOrdinal("DenialReason"))
                    ? null : reader.GetString(reader.GetOrdinal("DenialReason")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                    ? null : reader.GetString(reader.GetOrdinal("Notes"))
            };
        }
    }
}
