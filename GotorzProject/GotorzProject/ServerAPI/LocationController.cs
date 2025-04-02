using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace GotorzProject.ServerAPI
{
    [ApiController ]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        HttpClient Http;

        public LocationController(HttpClient http)
        {
            Http = http;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("getCountries")]
        public async Task<IActionResult> GetContries()
        {
            //Definerer headers
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://country-state-city-search-rest-api.p.rapidapi.com/allcountries")
            {
                Headers =
            {
                { "x-rapidapi-key", "89a426e5e5msh5e0f6acce251423p153feejsnb4c34d85e74b"},
                { "x-rapidapi-host", "country-state-city-search-rest-api.p.rapidapi.com" }
            }
            };

            var response = await Http.SendAsync(requestMessage);
            
            //Console.WriteLine(response.Headers);
            //Console.WriteLine(response.StatusCode);
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            var result = new List<string>();

            if (response.IsSuccessStatusCode)
            {
                var countriesList = await response.Content.ReadFromJsonAsync<List<Country>>();

                return Ok(countriesList.Select(c => c.Name).ToList());
            }
            else
            {
                return BadRequest("Failed to load countries");
            }
        }

    }
    public class Country
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isoCode")]
        public string IsoCode { get; set; }

        [JsonPropertyName("flag")]
        public string Flag { get; set; }

        [JsonPropertyName("phonecode")]
        public string PhoneCode { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        
        
    }
}
