using GotorzProject.Model;
using GotorzProject.Model.ObjectRelationMapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace GotorzProject.ServerAPI
{
    [Route("API/Auth/[controller]")]
    public class Authentication : Controller
    {

        PrimaryDbContext _context;
        
        public Authentication(PrimaryDbContext context)
        {
            _context = context;
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
            var user = _context.Customers.First((usr) => usr.Email == loginRequest.Email);

            if (user != null)
            {
                // verify password matches stored password
                bool correctPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
                if (correctPassword)
                {
                    // Todo : generate token here
                    string token = "";

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
        public IActionResult Register([FromBody] Customer registerRequest)
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

                _context.Customers.Add(customer);

                return Ok();
            }
        }
    }


    public class AuthRequest
    {
        
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
