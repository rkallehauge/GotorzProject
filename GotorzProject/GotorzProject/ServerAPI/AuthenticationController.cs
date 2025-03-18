using GotorzProject.Model;
using GotorzProject.Model.ObjectRelationMapping;
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
        
        public AuthenticationController(PrimaryDbContext context)
        {
            _context = context;
            Console.WriteLine("I am here!");
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthRequest loginRequest)
        {

            if (_context == null)
            {
                throw new InvalidConfigurationException("Bad configuration, code is ass, terminating session.");
            }

            // username is email
            // todo : change all places to say email instead of username
            var user = _context.Customers.FirstOrDefault((usr) => usr.Email == loginRequest.Email);

            if (user != null)
            {
                // verify password matches stored password
                bool correctPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
                if (correctPassword)
                {
                    // Todo : generate token here
                    string token = GenerateToken();
                    Response.Cookies.Append("AuthToken", token);

                    return Ok(token);
                }

                return BadRequest("Invalid login.");
            }
            else
            {
                return BadRequest("Invalid login.");
            }
        }
        
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            if(_context == null)
            {
                throw new InvalidConfigurationException("Bad configuration, code is ass, terminating session.");
            }

            var user = _context.Customers.First((usr) => usr.Email == registerRequest.Email);
            if (user != null)
            {
                // are we technically leaking whether a user exists by this?
                return BadRequest("Email already in use.");
            }
            else
            {
                // TODO : Add futher fields
                Customer customer = new();
                customer.Email = registerRequest.Email;
                customer.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(registerRequest.Password);

                customer.FirstName = registerRequest.FirstName;
                customer.LastName = registerRequest.LastName;
                customer.TelephoneNumber = registerRequest.Phone

                _context.Customers.Add(customer);

                return Ok();
            }
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Hello mr bob.");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello");
        }

        private static string GenerateToken()
        {
                
            byte[] buffer = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
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
