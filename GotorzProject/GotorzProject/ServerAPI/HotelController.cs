using GotorzProject.Service;
using GotorzProject.Service.System;
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
        DatabaseLogger _databaseLogger;

        public HotelController(IHotelProvider hotelProvider, DatabaseLogger databaseLogger)
        {
            _hotelProvider = hotelProvider;
            _databaseLogger = databaseLogger;
        }

        [HttpGet("GetHotels")]
        public async Task<IActionResult> GetHotels([FromQuery] string location, [FromQuery] string checkIn, [FromQuery] string checkOut)
        {
            try
            {
                if (_hotelProvider == null)
                    throw new InvalidConfigurationException("Hotel Provider was not provided 🤡");
            } catch (InvalidConfigurationException e)
            {
                _databaseLogger.LogCritical($"Critical error occured in HotelController");
                _databaseLogger.LogCritical($"{e.Message}");
            }

            DateOnly arrival = DateOnly.Parse(checkIn);
            DateOnly depart = DateOnly.Parse(checkOut);

            var result = await _hotelProvider.GetHotels(location, arrival, depart);

            if (result == null || result.Count() == 0)
            {
                _databaseLogger.LogInformation("GetHotels returned no results.");
                return BadRequest("No results found");
            }

            var json =  JsonSerializer.Serialize(result);

            return Ok(json);
        }
    }
}
