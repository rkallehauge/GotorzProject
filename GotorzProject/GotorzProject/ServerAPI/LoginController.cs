using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GotorzProject.Shared;
using GotorzProject.Model;
using GotorzProject.Service.System;

namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly DatabaseLogger _databaseLogger;

        public LoginController(IConfiguration configuration,
                               SignInManager<CustomUser> signInManager, DatabaseLogger databaseLogger)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _databaseLogger = databaseLogger;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);



            if (!result.Succeeded)
            {
                _databaseLogger.LogInformation($"Password attempt failed for user : {login.Email}");
                return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });
            }
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

            SymmetricSecurityKey key = default!;
            SigningCredentials creds = default!;
            DateTime expiry = default!;

            try
            {
                key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
            }
            catch (Exception ex)
            {
                _databaseLogger.LogCritical("Error occured during loading JWT token parameters.");
            }
            if (key != null && creds != null)
            {
                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires: expiry,
                    signingCredentials: creds
                );
                Console.WriteLine(token);
                _databaseLogger.LogInformation("User successfully logged in.");
                return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            else
            {
                _databaseLogger.LogCritical("System critical error occured during logging a user in.");
            }
            return BadRequest(new LoginResult { Successful = false, Error = "Error occured during login attempt." }); 
            // only way to reach here is if something went terribly wrong

        }
    }
}
