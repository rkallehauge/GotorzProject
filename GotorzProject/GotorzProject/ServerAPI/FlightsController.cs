using GotorzProject.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

        [HttpGet("Single")]
        public IActionResult SingleSearch()
        {

            var result = _flightProvider.GetFlights("bonka", "donka", DateOnly.Parse("24-03-2025"));
            StringBuilder sb = new();
            
            return Ok();
        }
    }
}
