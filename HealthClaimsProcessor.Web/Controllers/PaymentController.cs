using System;
using System.Web.Mvc;
using HealthClaimsProcessor.Core.Logging;
using HealthClaimsProcessor.Core.Models;
using HealthClaimsProcessor.Core.Services;

namespace HealthClaimsProcessor.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public ActionResult Index()
        {
            LoggingHelper.LogInfo("Listing all payments", "Controller");

            try
            {
                var payments = _paymentService.GetAllPayments();
                return View(payments);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Error listing payments", ex, "Controller");
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            LoggingHelper.LogInfo($"Viewing payment details for ID: {id}", "Controller");

            try
            {
                var payment = _paymentService.GetPaymentById(id);
                if (payment == null)
                {
                    return HttpNotFound();
                }
                return View(payment);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error viewing payment details for ID: {id}", ex, "Controller");
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Create(int claimId)
        {
            LoggingHelper.LogInfo($"Displaying payment form for claim ID: {claimId}", "Controller");

            var payment = new Payment { ClaimId = claimId };
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payment payment)
        {
            LoggingHelper.LogInfo($"Processing payment for claim ID: {payment.ClaimId}", "Controller");

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(payment);
                }

                int newId = _paymentService.ProcessPayment(payment);
                LoggingHelper.LogInfo($"Payment processed with ID: {newId}", "Controller");
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error processing payment for claim ID: {payment.ClaimId}", ex, "Controller");
                ModelState.AddModelError("", ex.Message);
                return View(payment);
            }
        }
    }
}
