using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class ProviderRepository
    {
        private readonly Database _database;

        public ProviderRepository()
        {
            _database = DatabaseFactory.GetDatabase();
        }

        public List<Provider> GetAllProviders()
        {
            LoggingHelper.LogInfo("Retrieving all providers", "DataAccess");

            var providers = new List<Provider>();

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetAllProviders"))
                using (IDataReader reader = _database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        providers.Add(MapProvider(reader));
                    }
                }

                LoggingHelper.LogInfo($"Retrieved {providers.Count} providers", "DataAccess");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all providers", ex, "DataAccess");
                throw;
            }

            return providers;
        }

        public Provider GetProviderById(int providerId)
        {
            LoggingHelper.LogInfo($"Retrieving provider with ID: {providerId}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_GetProviderById"))
                {
                    _database.AddInParameter(command, "@ProviderId", DbType.Int32, providerId);

                    using (IDataReader reader = _database.ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            var provider = MapProvider(reader);
                            LoggingHelper.LogInfo($"Found provider: {provider.Name}", "DataAccess");
                            return provider;
                        }
                    }
                }

                LoggingHelper.LogWarning($"Provider with ID {providerId} not found", "DataAccess");
                return null;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving provider with ID: {providerId}", ex, "DataAccess");
                throw;
            }
        }

        public int InsertProvider(Provider provider)
        {
            LoggingHelper.LogInfo($"Inserting new provider: {provider.Name}", "DataAccess");

            try
            {
                using (DbCommand command = _database.GetStoredProcCommand("sp_InsertProvider"))
                {
                    _database.AddOutParameter(command, "@ProviderId", DbType.Int32, 4);
                    _database.AddInParameter(command, "@Name", DbType.String, provider.Name);
                    _database.AddInParameter(command, "@NPI", DbType.String, provider.NPI);
                    _database.AddInParameter(command, "@TaxId", DbType.String, provider.TaxId);
                    _database.AddInParameter(command, "@Specialty", DbType.String, provider.Specialty);
                    _database.AddInParameter(command, "@Address", DbType.String, provider.Address);
                    _database.AddInParameter(command, "@City", DbType.String, provider.City);
                    _database.AddInParameter(command, "@State", DbType.String, provider.State);
                    _database.AddInParameter(command, "@ZipCode", DbType.String, provider.ZipCode);
                    _database.AddInParameter(command, "@Phone", DbType.String, provider.Phone);
                    _database.AddInParameter(command, "@Fax", DbType.String,
                        (object)provider.Fax ?? DBNull.Value);
                    _database.AddInParameter(command, "@Email", DbType.String,
                        (object)provider.Email ?? DBNull.Value);
                    _database.AddInParameter(command, "@IsNetworkProvider", DbType.Boolean, provider.IsNetworkProvider);
                    _database.AddInParameter(command, "@CredentialedDate", DbType.DateTime, provider.CredentialedDate);
                    _database.AddInParameter(command, "@CredentialExpirationDate", DbType.DateTime,
                        provider.CredentialExpirationDate.HasValue ? (object)provider.CredentialExpirationDate.Value : DBNull.Value);
                    _database.AddInParameter(command, "@IsActive", DbType.Boolean, provider.IsActive);

                    _database.ExecuteNonQuery(command);

                    int newId = (int)_database.GetParameterValue(command, "@ProviderId");
                    provider.ProviderId = newId;

                    LoggingHelper.LogInfo($"Inserted provider with ID: {newId}", "DataAccess");
                    return newId;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error inserting provider: {provider.Name}", ex, "DataAccess");
                throw;
            }
        }

        private Provider MapProvider(IDataReader reader)
        {
            return new Provider
            {
                ProviderId = reader.GetInt32(reader.GetOrdinal("ProviderId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                NPI = reader.GetString(reader.GetOrdinal("NPI")),
                TaxId = reader.GetString(reader.GetOrdinal("TaxId")),
                Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                City = reader.GetString(reader.GetOrdinal("City")),
                State = reader.GetString(reader.GetOrdinal("State")),
                ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                IsNetworkProvider = reader.GetBoolean(reader.GetOrdinal("IsNetworkProvider")),
                CredentialedDate = reader.GetDateTime(reader.GetOrdinal("CredentialedDate")),
                CredentialExpirationDate = reader.IsDBNull(reader.GetOrdinal("CredentialExpirationDate"))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal("CredentialExpirationDate")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
    }
}
