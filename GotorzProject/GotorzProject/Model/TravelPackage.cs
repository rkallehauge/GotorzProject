using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class TravelPackage
    {
        [Key]
        public int Id { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationCountry { get; set; }

        [Required]
        public Hotel? Hotel { get; set; }

        [Required]
        public FlightDeparture? FlightDeparture { get; set; }

        public double Price { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Return { get; set; }
        // for ef core
        public TravelPackage()
        {
        }

        //Midlertidig constructor med begrænset info
        public TravelPackage(string destinationCity, string destinationCountry)
        {
            DestinationCity = destinationCity;
            DestinationCountry = destinationCountry;
        }

        //Den egentlige constructor, når page virker
        public void UpdatedTravelPackage(int paymentID, string destinationCity, string destinationCountry, Hotel hotel, double price, DateTime departure, DateTime @return)
        {
            DestinationCity = destinationCity;
            DestinationCountry = destinationCountry;
            Hotel = hotel;
            Price = price;
            Departure = departure;
            Return = @return;
        }
    }
}
