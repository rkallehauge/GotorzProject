using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace GotorzProject.ServerAPI
{
    public class StripePaymentController : Controller
    {
        private readonly StripeClient _stripeClient;

        public StripePaymentController(StripeClient stripeClient)
        {
            _stripeClient = stripeClient;
        }

        [HttpPost ("payment/charge")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request) 
        {
            var paymentIntentService = new PaymentIntentService(_stripeClient);
            var options = new PaymentIntentCreateOptions
            {
              Amount = request.Amount,
              Currency = "dkk",

            };

            var paymentIntent = await paymentIntentService.CreateAsync(options);
            return Json(new { clientSecret = paymentIntent.ClientSecret });

        }

        public class PaymentRequest 
        {
            public long Amount { get; set; }
        }
    }
}
