using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace GotorzProject.ServerAPI.Controllers
{
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly HttpClient _httpClient;
        string ApiKey;
        public HotelController(HttpClient httpClient, IConfigurationRoot Config) 
        {
            
            _httpClient = httpClient;
            ApiKey = Config.GetValue<string>("Booking.com");        // todo: ret til rigtig APInøgle
        }
        [HttpPost("FromCityName")]
        public async Task<IActionResult> FromCityName()
        {
            

        }

       

    }
}
