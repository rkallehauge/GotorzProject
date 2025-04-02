using GotorzProject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Protocols.Configuration;
using System.Text.Json;

namespace GotorzProject.ServerAPI
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        IHotelProvider _hotelProvider;

        public HotelController(IHotelProvider hotelProvider)
        {
            _hotelProvider = hotelProvider;
        }

        [HttpGet("GetHotels")]
        public async Task<IActionResult> GetHotels([FromQuery] string location, [FromQuery] string checkIn, [FromQuery] string checkOut)
        {
            if (_hotelProvider == null)
                throw new InvalidConfigurationException("Hotel Provider was not provided 🤡");

            DateOnly arrival = DateOnly.Parse(checkIn);
            DateOnly depart = DateOnly.Parse(checkOut);

            var result = await _hotelProvider.GetHotels(location, arrival, depart);

            if (result == null || result.Count() == 0)
                return BadRequest("No results found");

            var json =  JsonSerializer.Serialize(result);

            return Ok(json);
        }
    }
}
