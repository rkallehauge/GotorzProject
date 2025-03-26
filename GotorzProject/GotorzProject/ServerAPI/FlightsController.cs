using GotorzProject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using GotorzProject.Shared.DataTransfer;

namespace GotorzProject.ServerAPI
{
    // absolute fucking wizardry
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {

        IFlightProvider _flightProvider;
        UserManager<IdentityUser> _userManager;

        public FlightsController(IFlightProvider flightProvider)
        {
            _flightProvider = flightProvider;
        }

        [HttpGet("Single")]
        public async Task<IActionResult> SingleSearch([FromQuery] string from, [FromQuery] string to, [FromQuery] string departure)
        {



            DateOnly dep = DateOnly.Parse(departure);

            var result = await _flightProvider.GetFlights(from, to, dep);

            var json = JsonSerializer.Serialize(result);
 
            return Ok(json);
        }
    }
}
