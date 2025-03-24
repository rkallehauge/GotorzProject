using GotorzProject.Model;

namespace GotorzProject.Service
{
    public interface IBookingFlightProvider
    {
        
        public Task<List<FlightDeparture>> GetFlights(string from, string to, DateOnly departureDate);

        // nice to have
        //List<FlightDeparture> GetFlightsMulti();
    }
}