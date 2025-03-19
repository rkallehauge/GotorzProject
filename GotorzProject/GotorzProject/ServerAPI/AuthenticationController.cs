using GotorzProject.Model;
using GotorzProject.Model.ObjectRelationMapping;
using GotorzProject.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        PrimaryDbContext _context;
        UserAuthenticationService _authenticationService;
        
        public AuthenticationController(PrimaryDbContext context, UserAuthenticationService userAuthService)
        {
            _context = context;
            _authenticationService = userAuthService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest loginRequest)
        {
            var result = await _authenticationService.AsyncLogin(loginRequest.Email, loginRequest.Password);
            if (!string.IsNullOrEmpty(result))
            {
                Response.Cookies.Append("AuthToken", result);
                return Ok();
            }

            return BadRequest("Invalid Login.");
        }
        
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            Customer customer = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                PostalCode = registerRequest.PostalCode,
                Address = registerRequest.Address,
                Country = registerRequest.Country,
                TelephoneNumber = registerRequest.PhoneNumber
            };

            var result = await _authenticationService.Register(customer);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Couldn't reigster user, try another email.");
        }

        [HttpGet("Info")]
        public async Task<IActionResult> Info()
        {
            // Todo : make field variable, as to not make easier to change
            Request.Cookies.TryGetValue("AuthToken", out string? token);

            //if (token == null) return BadRequest("No token provided");

            foreach(var cookie in Request.Cookies)
            {
                Console.WriteLine($"{cookie.Key} : {cookie.Value}");
            }
            
            return Ok("");
        }
    }
    public class AuthRequest
    {
        
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? PostalCode{ get; set; }
        public string? Address{ get; set; }
        public string? PhoneNumber{ get; set; }
        public string? Email{ get; set; }
        public string? Password{ get; set; }
    }
}
