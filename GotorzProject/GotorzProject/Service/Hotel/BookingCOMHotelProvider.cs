
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

            if (!_httpClient.DefaultRequestHeaders.Contains("x-rapidapi-key"))
            {
                _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apikey);
            }

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
                    throw new HttpRequestException($"Fejl: {response.StatusCode}");
                }

                SearchDestinationResponse hotels = await response.Content.ReadFromJsonAsync<SearchDestinationResponse>();

                if (hotels?.Data == null || hotels.Data.Count == 0)
                {
                    throw new Exception("Ingen destination fundet.");
                }

                var result = hotels.Data[0];
                string destinationId = result.Id;
                string searchType = result.Type;

                requestUrl = ApiBase + $"searchHotels?dest_id={destinationId}&search_type={searchType}";
                var response2 = await _httpClient.GetAsync(requestUrl);

                if (!response2.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Fejl ved hotel-søgning: {response2.StatusCode}");
                }

                HotelApiResponse hotelData = await response2.Content.ReadFromJsonAsync<HotelApiResponse>();

                if (hotelData?.Data?.Hotels == null || hotelData.Data.Hotels.Count == 0)
                {
                    throw new Exception("Ingen hoteller fundet.");
                }

                var hotelList = hotelData.Data.Hotels.Select(hotel => new HotelDataTransferObject
                {
                    Name = hotel.Property.Name,
                    PricePerNight = (int)hotel.Property.PriceBreakdown.GrossPrice.Value
                   

                }).ToList();

                return hotelList;
            }
            catch (Exception E)
            {
                Console.WriteLine($"Fejl: {E.Message}");
                return new List<HotelDataTransferObject>();
            }
        }


        public async Task<List<HotelDataTransferObject>> GetHotelsFromCityNameAndPrice(string cityName, double price)         //lav kun en maks pris på rejser 
        {
            string requestUrl = ApiBase + $"searchDestination?query={cityName}";

            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apikey);

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Fejl: {response.StatusCode}");
                }

                SearchDestinationResponse hotels = await response.Content.ReadFromJsonAsync<SearchDestinationResponse>();

                if (hotels?.Data == null || hotels.Data.Count == 0)
                {
                    throw new Exception("Ingen destination fundet.");
                }

                var result = hotels.Data[0];
                string destinationId = result.Id;
                string searchType = result.Type;

                requestUrl = ApiBase + $"searchHotels?dest_id={destinationId}&search_type={searchType}";
                var response2 = await _httpClient.GetAsync(requestUrl);

                if (!response2.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Fejl ved hotel-søgning: {response2.StatusCode}");
                }


                HotelApiResponse hotelData = await response2.Content.ReadFromJsonAsync<HotelApiResponse>();

                if (hotelData?.Data.Hotels == null || hotelData.Data.Hotels.Count == 0)
                {
                    throw new Exception("Ingen hoteller fundet.");
                }


                var hotelList = hotelData.Data.Hotels.Select(hotel => new HotelDataTransferObject
                {
                    Name = hotel.Property.Name,
                    PricePerNight = (int)hotel.Property.PriceBreakdown.GrossPrice.Value
                }).ToList();

                return hotelList;
            }
            catch (Exception E)
            {
                Console.WriteLine($"Fejl: {E.Message}");
                return new List<HotelDataTransferObject>();
            }
        }

    }


     public class HotelApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public List<Hotel> Hotels { get; set; }
    }

    public class Hotel
    {
        public int HotelId { get; set; }
        public string AccessibilityLabel { get; set; }
        public Property Property { get; set; }
    }

    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ufi { get; set; }
        public CheckinCheckout Checkin { get; set; }
        public CheckinCheckout Checkout { get; set; }
        public string CheckinDate { get; set; }
        public string CheckoutDate { get; set; }
        public int QualityClass { get; set; }
        public int AccuratePropertyClass { get; set; }
        public int PropertyClass { get; set; }
        public int MainPhotoId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int RankingPosition { get; set; }
        public double ReviewScore { get; set; }
        public string ReviewScoreWord { get; set; }
        public int ReviewCount { get; set; }
        public string CountryCode { get; set; }
        public bool IsPreferred { get; set; }
        public bool IsFirstPage { get; set; }
        public string Currency { get; set; }
        public string WishlistName { get; set; }
        public List<string> BlockIds { get; set; }
        public List<string> PhotoUrls { get; set; }
        public PriceBreakdown PriceBreakdown { get; set; }
    }

    public class CheckinCheckout
    {
        public string FromTime { get; set; }
        public string UntilTime { get; set; }
    }

    public class PriceBreakdown
    {
        public PriceDetail GrossPrice { get; set; }
        public PriceDetail ExcludedPrice { get; set; }
        public List<object> TaxExceptions { get; set; }
        public List<BenefitBadge> BenefitBadges { get; set; }
        public PriceDetail StrikethroughPrice { get; set; }
    }

    public class PriceDetail
    {
        public string Currency { get; set; }
        public double Value { get; set; }
    }

    public class BenefitBadge
    {
        public string Explanation { get; set; }
        public string Identifier { get; set; }
        public string Text { get; set; }
        public string Variant { get; set; }
    }


    public class SearchDestinationResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("data")]
        public List<LocationData> Data { get; set; }
    }

    public class LocationData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("cityName")]
        public string CityName { get; set; }

        [JsonPropertyName("regionName")]
        public string RegionName { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("countryName")]
        public string CountryName { get; set; }

        [JsonPropertyName("countryNameShort")]
        public string CountryNameShort { get; set; }

        [JsonPropertyName("photoUri")]
        public string PhotoUri { get; set; }

        [JsonPropertyName("distanceToCity")]
        public Distance DistanceToCity { get; set; }

        [JsonPropertyName("parent")]
        public string Parent { get; set; }
    }

    public class Distance
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }
    }
}
