using System.ComponentModel.DataAnnotations;

namespace GotorzProject.Model
{
    public class FlightDeparture
    {
        [Key]
        public int Id { get; set; }

        //todo: switch with paymentmodel
        public int PaymentID { get; set; }  

        public string Airline { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }

        public DateTime DepartureDate { get; set;}
        public DateTime ArrivalDate { get; set; }


        public FlightDeparture(int paymentID, string airline, string departureAirport, string arrivalAirport, DateTime departureDate, DateTime arrivalDate)
        {
            PaymentID = paymentID;
            Airline = airline;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
        }


    }
}
