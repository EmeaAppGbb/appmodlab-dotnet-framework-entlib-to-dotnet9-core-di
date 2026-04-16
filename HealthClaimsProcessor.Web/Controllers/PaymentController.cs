using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Listing all payments");

            try
            {
                var payments = _paymentService.GetAllPayments();
                return View(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing payments");
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Viewing payment details for ID: {PaymentId}", id);

            try
            {
                var payment = _paymentService.GetPaymentById(id);
                if (payment == null)
                {
                    return NotFound();
                }
                return View(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing payment details for ID: {PaymentId}", id);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create(int claimId)
        {
            _logger.LogInformation("Displaying payment form for claim ID: {ClaimId}", claimId);

            var payment = new Payment { ClaimId = claimId };
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Payment payment)
        {
            _logger.LogInformation("Processing payment for claim ID: {ClaimId}", payment.ClaimId);

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(payment);
                }

                int newId = _paymentService.ProcessPayment(payment);
                _logger.LogInformation("Payment processed with ID: {PaymentId}", newId);
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for claim ID: {ClaimId}", payment.ClaimId);
                ModelState.AddModelError("", ex.Message);
                return View(payment);
            }
        }
    }
}
