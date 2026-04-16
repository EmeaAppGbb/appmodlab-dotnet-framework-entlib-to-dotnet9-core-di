using System;
using System.Web.Mvc;
using HealthClaimsProcessor.Core.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace HealthClaimsProcessor.Web.Filters
{
    public class EntLibExceptionFilter : IExceptionFilter
    {
        private readonly ExceptionManager _exceptionManager;

        public EntLibExceptionFilter()
        {
            var factory = new ExceptionPolicyFactory(new Microsoft.Practices.EnterpriseLibrary.Common.Configuration.SystemConfigurationSource());
            _exceptionManager = factory.CreateManager();
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null || filterContext.ExceptionHandled)
            {
                return;
            }

            var exception = filterContext.Exception;
            LoggingHelper.LogError($"Exception in {filterContext.RouteData.Values["controller"]}.{filterContext.RouteData.Values["action"]}", 
                exception, "Controller");

            bool rethrow = _exceptionManager.HandleException(exception, "Global Policy");

            if (!rethrow)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(
                        new HandleErrorInfo(exception, 
                            filterContext.RouteData.Values["controller"]?.ToString() ?? "Unknown",
                            filterContext.RouteData.Values["action"]?.ToString() ?? "Unknown"))
                };
            }
        }
    }
}
