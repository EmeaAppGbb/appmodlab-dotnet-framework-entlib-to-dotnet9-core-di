using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HealthClaimsProcessor.Core.Logging;

namespace HealthClaimsProcessor.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            LoggingHelper.LogInfo("Health Claims Processor application starting", "Application");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();

            LoggingHelper.LogInfo("Health Claims Processor application started successfully", "Application");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                LoggingHelper.LogError("Unhandled application error", exception, "Application");
            }
        }

        protected void Application_End()
        {
            LoggingHelper.LogInfo("Health Claims Processor application shutting down", "Application");
        }
    }
}
