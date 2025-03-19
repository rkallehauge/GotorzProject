namespace GotorzProject.Service.Hotel
{
    public interface IHotelProvider
    {
        public Task<List<HotelDataTransferObject>> GetHotelsFromCityName(string cityName);

        public Task<List<HotelDataTransferObject>> GetHotelsFromCityNameAndPrice(string cityName, double price);

    }
}
