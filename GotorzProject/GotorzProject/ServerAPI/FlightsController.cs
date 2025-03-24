using GotorzProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace GotorzProject.ServerAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {

        IBookingFlightProvider _flightProvider;

        public FlightsController(IBookingFlightProvider flightProvider)
        {

            _flightProvider = flightProvider;
        }

        public IActionResult Index()
        {

            _flightProvider.GetFlights("Bomba", "Donka", DateOnly.Parse("24-03-2025"), DateOnly.Parse("26-03-2025"));

            return Ok();
        }
    }
}
