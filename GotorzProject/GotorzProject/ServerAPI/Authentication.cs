using GotorzProject.Model.ObjectRelationMapping;
using Microsoft.AspNetCore.Mvc;

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

            // username is email
            // todo : change all places to say email instead of username
            var users = _context.Customers.First((usr) => usr.Email == loginRequest.Username);

            


            return BadRequest("Lol");
        }


        [HttpPost("Register")]
        public IActionResult Register([FromBody] AuthRequest loginRequest)
        {

            //var users = _context.



            return BadRequest("Lol");
        }
    }


    public class AuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
