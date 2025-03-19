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



                string hash = user.Password ?? "";

                Console.WriteLine($"{loginRequest.Password} {loginRequest.Password.Length} {hash} length {hash.Length}");
                bool correctPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, hash);
                if (correctPassword)
                {
                    // Todo : generate token here
                    string token = GenerateToken();
                    Response.Cookies.Append("AuthToken", token);

                    return Ok(token);
                }

                return BadRequest("Invalid login. type a");
            }
            else
            {
                return BadRequest("Invalid login. type b");
            }
        }
        
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            if(_context == null)
            {
                throw new InvalidConfigurationException("Bad configuration, code is ass, terminating session.");
            }

            bool exists = _context.Customers.Any((cust) =>  cust.Email == registerRequest.Email);
            if (exists)
            {
                // are we technically leaking whether a user exists by this?
                return BadRequest("Email already in use.");
            }
            else
            {
                // TODO : Add futher fields
                Customer customer = new();

                Console.WriteLine($"Em : {registerRequest.Email} pw : {registerRequest.Password} pwlength : {registerRequest.Password.Length}");

                customer.Email = registerRequest.Email;
                customer.Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                //customer.FirstName = registerRequest.FirstName;
                //customer.LastName = registerRequest.LastName;
                //customer.TelephoneNumber = registerRequest.Phone
                Console.WriteLine(customer);
                _context.Customers.Add(customer);

                _context.SaveChanges();

                return Ok();
            }
        }

        [HttpGet("Info")]
        public async Task<IActionResult> Info()
        {
            // Todo : make field variable, as to not make easier to change
            Request.Cookies.TryGetValue("AuthToken", out string? token);

            if (token == null) return BadRequest("No token provided");


            return Ok(":)");
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
