//skal have en httpklient for at kunne tale med api. Spørge i program.cs
//to api nøgler (hem og off)
//

using Stripe;
using System.Net.Http;

namespace GotorzProject.Service
{
    public class PaymentProvider : IPaymentProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PaymentProvider> _logger;
        private readonly StripeClient _stripeClient;

        public PaymentProvider(IHttpClientFactory httpClientFactory,
                               ILogger<PaymentProvider> logger,
                               StripeClient stripeClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _stripeClient = stripeClient;
        
        }
          

        public async Task<string> CreatePaymentIntentAsync(long amount, string currency) 
        {
            var paymentIntentService = new PaymentIntentService();

            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
            };
            try
            {
                var paymentIntent = await paymentIntentService.CreateAsync(options);
                return paymentIntent.ClientSecret;
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl under oprettelse", ex);
                return string.Empty;
            
            }
        }


    }
}
