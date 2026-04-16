using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home page accessed");
            ViewBag.Message = "Welcome to the Health Claims Processor";
            return View();
        }
    }
}
