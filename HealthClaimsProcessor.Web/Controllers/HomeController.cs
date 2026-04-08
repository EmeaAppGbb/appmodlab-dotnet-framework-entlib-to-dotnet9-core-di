using System.Web.Mvc;
using HealthClaimsProcessor.Core.Logging;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LoggingHelper.LogInfo("Home page accessed", "Controller");
            ViewBag.Message = "Welcome to the Health Claims Processor";
            return View();
        }
    }
}
