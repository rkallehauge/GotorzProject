using GotorzProject.Service;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace GotorzProject.ServerAPI
{
    [Route("payment")]
    public class StripePaymentController : Controller
    {
        private readonly IPaymentProvider _paymentProvider;

        public StripePaymentController(IPaymentProvider paymentProvider)
        {
            _paymentProvider = paymentProvider;
        }

        [HttpPost ("charge")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request) 
        {
            if (request == null || request.Amount <= 0) 
            {
                return BadRequest("Beløbet skal være positivt");
            }
            var clientSecret = await _paymentProvider.CreatePaymentIntentAsync(request.Amount, "dkk");

            if (string.IsNullOrEmpty(clientSecret))
            {
                return StatusCode(500, "Fejl ved oprettelse af betaling");
            }
            return Json(new { clientSecret });
        }

        public class PaymentRequest 
        {
            public long Amount { get; set; }
        }
    }
}
