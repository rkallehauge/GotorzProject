using GotorzProject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GotorzProject.ServerAPI
{
    // absolute fucking wizardry
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {

        IBookingFlightProvider _flightProvider;
        UserManager<IdentityUser> _userManager;

        public FlightsController(IBookingFlightProvider flightProvider)
        {
            _flightProvider = flightProvider;
        }

        [HttpGet("Single")]
        public IActionResult SingleSearch()
        {
            foreach(var item in Request.Headers.Authorization)
            {
                Console.WriteLine(item);
            }
            //var result = _flightProvider.GetFlights("bonka", "donka", DateOnly.Parse("24-03-2025"));
            StringBuilder sb = new();
            
            return Ok();
        }
    }
}
