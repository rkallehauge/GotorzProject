using GotorzProject.Service.Model;
using GotorzProject.Shared;
using GotorzProject.Shared.DataTransfer;

namespace GotorzProject.Service
{
    public class BookingCOMHotelProvider : IHotelProvider
    {
        string dateFormat = "dd-MM-yyyy";
        HttpClient _httpClient;

        private readonly string apiBase;


        public BookingCOMHotelProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BookingCOM");
            apiBase = "/api/v1/hotels/";
        }


        // get hotels from generic search query
        public async Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(string location, DateOnly checkin, DateOnly checkout)
        {
            string locationSearchEndpoint = apiBase + "searchDestination";
            string hotelSearchEndpoint = apiBase + "searchHotels";

            string locationQuery = $"{locationSearchEndpoint}?query={location}";

            var locationResponse = await _httpClient.GetAsync(locationQuery);
            LocationSearchModel? lsm = await locationResponse.Content.ReadFromJsonAsync<LocationSearchModel>();
            if (lsm == null || lsm.Status != true || lsm.Data.Count==0)
            {
                return null;
            }

            string destId = lsm.Data.First().DestId;
            string searchType = lsm.Data.First().SearchType;

            string checkIn = checkin.ToString(dateFormat);
            string checkOut = checkout.ToString(dateFormat);


            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"dest_id", destId},
                {"search_type", searchType },

                { "arrival_date", checkIn },
                { "departure_date", checkOut}
            };

            var hotelQuery = hotelSearchEndpoint + Helper.ToQueryString(param);





        }

        public Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(int destinationId, DateOnly checkin, DateOnly checkout)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(string city, string country, DateOnly checkin, DateOnly checkout)
        {
            throw new NotImplementedException();
        }
    }
}
