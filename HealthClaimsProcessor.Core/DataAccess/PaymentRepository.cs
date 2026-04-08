using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class PaymentRepository
    {
        private readonly Database _database;

        public PaymentRepository()
        {
            _database = DatabaseFactory.GetDatabase();
        }

        public List<Payment> GetAllPayments()
        {
            LoggingHelper.LogInfo("Retrieving all payments", "DataAccess");

            var payments = new List<Payment>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetAllPayments"))
                using (IDataReader reader = _database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        payments.Add(MapPayment(reader));
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {payments.Count} payments", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all payments", ex, "DataAccess");
                throw;
            }

            return payments;
        }

        public Payment GetPaymentById(int paymentId)
        {
            LoggingHelper.LogInfo($"Retrieving payment with ID: {paymentId}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetPaymentById"))
                {
                    _database.AddInParameter(command, "@PaymentId", DbType.Int32, paymentId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            var payment = MapPayment(reader);
                            LoggingHelper.LogInfo($"Found payment: {payment.PaymentNumber}", "DataAccess");
                            return payment;
                        }
                    }
                }

                LoggingHelper.LogWarning($"Payment with ID {paymentId} not found", "DataAccess");
                return null;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving payment with ID: {paymentId}", ex, "DataAccess");
                throw;
            }
        }

        public List<Payment> GetPaymentsByClaimId(int claimId)
        {
            LoggingHelper.LogInfo($"Retrieving payments for claim ID: {claimId}", "DataAccess");

            var payments = new List<Payment>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetPaymentsByClaimId"))
                {
                    _database.AddInParameter(command, "@ClaimId", DbType.Int32, claimId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            payments.Add(MapPayment(reader));
                        }
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {payments.Count} payments for claim ID: {claimId}", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving payments for claim ID: {claimId}", ex, "DataAccess");
                throw;
            }

            return payments;
        }

        public int InsertPayment(Payment payment)
        {
            LoggingHelper.LogInfo($"Inserting new payment: {payment.PaymentNumber} for claim ID: {payment.ClaimId}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_InsertPayment"))
                {
                    _database.AddOutParameter(command, "@PaymentId", DbType.Int32, 4);
                    _database.AddInParameter(command, "@ClaimId", DbType.Int32, payment.ClaimId);
                    _database.AddInParameter(command, "@PaymentNumber", DbType.String, payment.PaymentNumber);
                    _database.AddInParameter(command, "@PaymentAmount", DbType.Decimal, payment.PaymentAmount);
                    _database.AddInParameter(command, "@PaymentDate", DbType.DateTime, payment.PaymentDate);
                    _database.AddInParameter(command, "@PaymentMethod", DbType.String, payment.PaymentMethod);
                    _database.AddInParameter(command, "@CheckNumber", DbType.String,
                        (object)payment.CheckNumber ?? DBNull.Value);
                    _database.AddInParameter(command, "@EFTTraceNumber", DbType.String,
                        (object)payment.EFTTraceNumber ?? DBNull.Value);
                    _database.AddInParameter(command, "@PayeeName", DbType.String,
                        (object)payment.PayeeName ?? DBNull.Value);
                    _database.AddInParameter(command, "@PayeeAddress", DbType.String,
                        (object)payment.PayeeAddress ?? DBNull.Value);
                    _database.AddInParameter(command, "@PaymentStatus", DbType.String, payment.PaymentStatus);
                    _database.AddInParameter(command, "@RemittanceAdviceNumber", DbType.String,
                        (object)payment.RemittanceAdviceNumber ?? DBNull.Value);
                    _database.AddInParameter(command, "@CreatedBy", DbType.String,
                        (object)payment.CreatedBy ?? DBNull.Value);

                    _database.ExecuteNonQuery(command);

                    int newId = (int)_database.GetParameterValue(command, "@PaymentId");
                    payment.PaymentId = newId;

                    LoggingHelper.LogInfo($"Inserted payment with ID: {newId}", "DataAccess");
                    return newId;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error inserting payment: {payment.PaymentNumber}", ex, "DataAccess");
                throw;
            }
        }

        public void UpdatePaymentStatus(int paymentId, string status, DateTime? clearedDate, string modifiedBy)
        {
            LoggingHelper.LogInfo($"Updating payment status for payment ID: {paymentId} to {status}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_UpdatePaymentStatus"))
                {
                    _database.AddInParameter(command, "@PaymentId", DbType.Int32, paymentId);
                    _database.AddInParameter(command, "@PaymentStatus", DbType.String, status);
                    _database.AddInParameter(command, "@ClearedDate", DbType.DateTime,
                        clearedDate.HasValue ? (object)clearedDate.Value : DBNull.Value);
                    _database.AddInParameter(command, "@ModifiedBy", DbType.String,
                        (object)modifiedBy ?? DBNull.Value);

                    _database.ExecuteNonQuery(command);
                }

                LoggingHelper.LogInfo($"Updated payment ID: {paymentId} status to {status}", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error updating payment status for payment ID: {paymentId}", ex, "DataAccess");
                throw;
            }
        }

        private Payment MapPayment(IDataReader reader)
        {
            return new Payment
            {
                PaymentId = reader.GetInt32(reader.GetOrdinal("PaymentId")),
                ClaimId = reader.GetInt32(reader.GetOrdinal("ClaimId")),
                PaymentNumber = reader.GetString(reader.GetOrdinal("PaymentNumber")),
                PaymentAmount = reader.GetDecimal(reader.GetOrdinal("PaymentAmount")),
                PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                CheckNumber = reader.IsDBNull(reader.GetOrdinal("CheckNumber"))
                    ? null : reader.GetString(reader.GetOrdinal("CheckNumber")),
                EFTTraceNumber = reader.IsDBNull(reader.GetOrdinal("EFTTraceNumber"))
                    ? null : reader.GetString(reader.GetOrdinal("EFTTraceNumber")),
                PayeeName = reader.IsDBNull(reader.GetOrdinal("PayeeName"))
                    ? null : reader.GetString(reader.GetOrdinal("PayeeName")),
                PayeeAddress = reader.IsDBNull(reader.GetOrdinal("PayeeAddress"))
                    ? null : reader.GetString(reader.GetOrdinal("PayeeAddress")),
                PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                ClearedDate = reader.IsDBNull(reader.GetOrdinal("ClearedDate"))
                    ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ClearedDate")),
                RemittanceAdviceNumber = reader.IsDBNull(reader.GetOrdinal("RemittanceAdviceNumber"))
                    ? null : reader.GetString(reader.GetOrdinal("RemittanceAdviceNumber")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))
                    ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy"))
                    ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy"))
                    ? null : reader.GetString(reader.GetOrdinal("ModifiedBy"))
            };
        }
    }
}
