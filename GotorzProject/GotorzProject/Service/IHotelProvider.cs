using GotorzProject.Shared.DataTransfer;

namespace GotorzProject.Service
{
    public interface IHotelProvider
    {
        // location here is generic search
        public Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(string location, DateOnly checkin, DateOnly checkout);

        // get hotels from city and country
        public Task<IEnumerable<BaseHotelRoomDTO>> GetHotels(string city, string country, DateOnly checkin, DateOnly checkout)

        // further shit needed probably


    }
}
