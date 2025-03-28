using System.Text.Json.Serialization;

namespace GotorzProject.Service.Model
{
    public class LocationSearchModel
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("data")]
        public List<CityData> Data { get; set; }
    }

    public class CityData
    {
        [JsonPropertyName("dest_id")]
        public string DestId { get; set; }

        [JsonPropertyName("search_type")]
        public string SearchType { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("lc")]
        public string LanguageCode { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cc1")]
        public string CountryCode { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("city_name")]
        public string CityName { get; set; }

        [JsonPropertyName("roundtrip")]
        public string Roundtrip { get; set; }

        [JsonPropertyName("nr_hotels")]
        public int? NumberOfHotels { get; set; }

        [JsonPropertyName("hotels")]
        public int? Hotels { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("dest_type")]
        public string DestinationType { get; set; }

        [JsonPropertyName("city_ufi")]
        public object CityUfi { get; set; }
    }

}
