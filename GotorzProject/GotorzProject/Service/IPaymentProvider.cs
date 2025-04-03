using GotorzProject.Client.Pages;
using GotorzProject.Shared.DataTransfer;
using GotorzProject.Service;

namespace GotorzProject.Service
{
    public interface IPaymentProvider
    {
        Task<string> CreatePaymentIntentAsync (long amount, string currency);

          
        
    }
}
