using GotorzProject.Model;
using GotorzProject.Shared.DataTransfer;

namespace GotorzProject.Service
{
    public interface IFlightProvider
    {
        
        public Task<List<BaseFlightDTO>> GetFlights(string from, string to, DateOnly departureDate);

        // nice to have
        //List<FlightDeparture> GetFlightsMulti();
    }
}