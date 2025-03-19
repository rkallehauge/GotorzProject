namespace GotorzProject.Service.Hotel
{
    public class HotelDataTransferObject
    {

        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? Stars { get; set; }
        public int? PricePerNight { get; set; }
        public int? NumberOfRooms { get; set; }

        public string? Currency {  get; set; }

        public List<string>? Amenities { get; set; }
        
    }
}
