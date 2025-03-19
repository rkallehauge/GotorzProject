
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace GotorzProject.Service.Hotel
{
    public class BookingCOMHotelProvider : IHotelProvider
    {
        string ApiBase = "https://booking-com15.p.rapidapi.com/api/v1/hotels/";
        HttpClient _httpClient;
        string _apikey;

        public BookingCOMHotelProvider(HttpClient httpClient, string apikey)
        {
            _httpClient = httpClient;
            _apikey = apikey;
        }

        public async Task<List<HotelDataTransferObject>> GetHotelsFromCityName(string cityName)
        {
            string requestUrl = ApiBase + $"searchDestination?query={cityName}";

            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apikey);

            try 
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode) 
                {
                    throw new HttpRequestException($"Fejl{response.StatusCode}");
                }

                ApiResponse hotels = await response.Content.ReadFromJsonAsync<ApiResponse>();   //Hvor er vi henne i verden? API
                var result = hotels.Data[0];
                string DestinationId, SearchType;
                DestinationId = result.DestId;          
                SearchType = result.DestType;

                requestUrl = ApiBase + $"searchHotels?dest_id={DestinationId}&search_type={SearchType}";
                var response2 = _httpClient.GetAsync(requestUrl);


            }
            catch(Exception E)
            {
                
            }


        }

        public Task<List<HotelDataTransferObject>> GetHotelsFromCityNameAndPrice(string cityName, double price)
        {
            throw new NotImplementedException();  //lav kun en maks pris på rejser
        }





    }

    public class ApiResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("data")]
        public List<Destination> Data { get; set; }
    }

    public class Destination
    {
        [JsonPropertyName("dest_id")]
        public string DestId { get; set; }

        [JsonPropertyName("search_type")]
        public string SearchType { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("cc1")]
        public string CountryCode { get; set; }

        [JsonPropertyName("dest_type")]
        public string DestType { get; set; }

        [JsonPropertyName("city_name")]
        public string CityName { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("lc")]
        public string LanguageCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("nr_hotels")]
        public int NumberOfHotels { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("roundtrip")]
        public string RoundTrip { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("hotels")]
        public int Hotels { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("city_ufi")]
        public int? CityUfi { get; set; }
    }
}
