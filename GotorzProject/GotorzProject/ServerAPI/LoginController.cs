using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GotorzProject.Shared;
using GotorzProject.Model;

namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        public LoginController(IConfiguration configuration,
                               SignInManager<CustomUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, login.Email)
            };
          
            

            // get roles for user and append them to claims
            foreach (var claim in _signInManager.Context.User.Claims)
            {
                if(claim.Type == ClaimTypes.Role)
                {
                    claims.Add(claim);
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
            //var expiry = DateTime.Now.AddSeconds(25); // apprently DateTime.Now is timezone relevant? -2 hours are "added" to expiry for some reason

            Console.WriteLine($"claims.Length {claims.Count}");

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            Console.WriteLine(token);

            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
