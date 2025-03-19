
namespace GotorzProject.Service.Hotel
{
    public class BookingCOMHotelProvider : IHotelProvider
    {

        HttpClient _httpClient;
        string _apikey;

        public BookingCOMHotelProvider(HttpClient httpClient, string apikey)
        {
            _httpClient = httpClient;
            _apikey = apikey;
        }

        public Task<List<HotelDataTransferObject>> GetHotelsFromCityName(string cityName)
        {
            throw new NotImplementedException();
        }

        public Task<List<HotelDataTransferObject>> GetHotelsFromCityNameAndPrice(string cityName, double price)
        {
            throw new NotImplementedException();
        }
    }
}
