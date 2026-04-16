using System.Web.Mvc;
using HealthClaimsProcessor.Web.Filters;

namespace HealthClaimsProcessor.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new EntLibExceptionFilter());
        }
    }
}
