using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared.DataTransfer
{
    public class BaseFlightDTO
    {

        // Oneway / Roundtrip
        public string Type { get; set; }

        public double Price { get; set; }
        public string CabinClass { get; set; }

        public string StartAirport { get; set; }
        public string EndAirport { get; set; }
        public List<FlightLeg> FlightLegs { get; set; }
    }
    // Flight segments should maybe be added
    // Flight segment is entire trip one way, and another segment is the entire way home
    // Flight leg is just one flight, or one leg of a trip
    public class FlightLeg
    {
        public Airline Carrier { get; set; }

        public DateTime Departure { get; set; }
        public DateTime Landing { get; set; }

        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }

        public string FromAirportName { get; set; }
        public string ToAirportName { get; set; }

        public int TravelTimeMinutes { get; set; }
    }

    public class Airline
    {
        public string Name { get; set; }
        public string Iata { get; set; }
        public string IconWebSource { get; set; }
    }
}
