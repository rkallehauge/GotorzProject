using GotorzProject.Model;
using Microsoft.AspNetCore.Mvc;

namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelPackageController : Controller
    {
        //Midlertidig liste til at teste med
        private static List<TravelPackage> _travelPackages = new List<TravelPackage>();

        [HttpPost]
        public IActionResult CreateTravelPackage([FromBody] TravelPackage travelPackage)
        {
            if (travelPackage == null)
            {
                return BadRequest("Invalid travel package");
            }

            _travelPackages.Add(travelPackage);

            return Ok($"Package created for {travelPackage.DestinationCity}, {travelPackage.DestinationCountry}");

        }

        [HttpGet]
        public IActionResult GetTravelPackage()
        {
            return Ok(_travelPackages);
        }

        [HttpPost("createPackage")]
        public IActionResult CreatePackage()
        {
            // Create a new package
            return Ok();
        }
    }
}
