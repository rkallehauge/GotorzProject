using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication;
using NuGet.Versioning;
using static System.Net.WebRequestMethods;

namespace GotorzProject.ServerAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        HttpClient Http;

        public LocationController(IHttpClientFactory factory)
        {
            // Get setup http client with headers from configuration
            Http = factory.CreateClient("Location");
        }

        // unused 
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("getCountries")]
        public async Task<IActionResult> GetContries()
        {

            // Get countries 
            var response = await Http.GetAsync("/allcountries");

            var result = new List<string>();

            Console.WriteLine(await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                var countriesList = await response.Content.ReadFromJsonAsync<List<Country>>();

                return Ok(countriesList);
            }
            else
            {
                return BadRequest("Failed to load countries");
            }
        }

        [HttpGet("getCities")]
        public async Task<IActionResult> GetCities([FromQuery] string country)
        {
            // Set query string with parameter from method
            string query = $"/cities-by-countrycode?countrycode={country}";

            // get response object
            var response = await Http.GetAsync(query);

            // parse response content into list of cities
            var result = await response.Content.ReadFromJsonAsync<List<City>>();

            // Convert list of cities into list of strings, each index is a city name
            var list = result.Select(c => c.name).ToList();
            
            // return list of citynames
            return Ok(list);
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

    public class City
    {
        public string name { get; set; }
        public string countryCode { get; set; }
        public string stateCode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

}
