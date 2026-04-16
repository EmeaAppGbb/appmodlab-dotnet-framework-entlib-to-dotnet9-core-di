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
    public class ProviderService
    {
        private readonly ProviderRepository _providerRepository;
        private readonly ExceptionManager _exceptionManager;

        public ProviderService(ProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
            _exceptionManager = new ExceptionPolicyFactory(new SystemConfigurationSource()).CreateManager();
        }

        public List<Provider> GetAllProviders()
        {
            LoggingHelper.LogInfo("Getting all providers", "Service");

            try
            {
                return _providerRepository.GetAllProviders();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error retrieving all providers", ex, "Service");

                Exception exceptionToThrow;
                if (_exceptionManager.HandleException(ex, "Data Access Policy", out exceptionToThrow))
                {
                    if (exceptionToThrow != null)
                        throw exceptionToThrow;
                    else
                        throw;
                }

                return new List<Provider>();
            }
        }

        public Provider GetProviderById(int providerId)
        {
            LoggingHelper.LogInfo($"Getting provider by ID: {providerId}", "Service");

            try
            {
                return _providerRepository.GetProviderById(providerId);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error retrieving provider with ID: {providerId}", ex, "Service");

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

        public int CreateProvider(Provider provider)
        {
            LoggingHelper.LogInfo("Creating new provider", "Service");

            try
            {
                var validator = ValidationFactory.CreateValidator<Provider>();
                var validationResults = validator.Validate(provider);
                if (!validationResults.IsValid)
                {
                    var messages = new List<string>();
                    foreach (var result in validationResults)
                    {
                        messages.Add(result.Message);
                    }
                    string errorMessage = "Provider validation failed: " + string.Join("; ", messages);
                    LoggingHelper.LogWarning(errorMessage, "Service");
                    throw new ValidationException(errorMessage);
                }

                provider.CreatedDate = DateTime.Now;
                provider.IsActive = true;

                int newId = _providerRepository.InsertProvider(provider);
                LoggingHelper.LogInfo($"Provider created successfully with ID: {newId}", "Service");
                return newId;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error creating provider", ex, "Service");

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

        private class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }
    }
}
