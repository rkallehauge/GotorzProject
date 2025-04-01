using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class TravelPackage
    {
        [Key]
        public int Id { get; set; }

        public int PaymentID { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationCountry { get; set; }

        [Required]
        public Hotel? Hotel { get; set; }

        public double Price { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Return { get; set; }
        // for ef core
        public TravelPackage()
        {
        }

        public TravelPackage(int paymentID, string destinationCity, string destinationCountry, Hotel hotel, double price, DateTime departure, DateTime @return)
        {
            PaymentID = paymentID;
            DestinationCity = destinationCity;
            DestinationCountry = destinationCountry;
            Hotel = hotel;
            Price = price;
            Departure = departure;
            Return = @return;
        }
    }
}
