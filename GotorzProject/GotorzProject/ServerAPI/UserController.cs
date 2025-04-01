using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using GotorzProject.Model;
using GotorzProject.Shared.DataTransfer;
using GotorzProject.Service;
using System.Text.Json;


namespace GotorzProject.ServerAPI
{

    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/[controller]")]

    public class UserController : Controller
    {

        IUserAdminstration _userService;

        public UserController(IUserAdminstration service)
        {
           _userService = service;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetAllUser()
        {
            await _userService.SetupRoles();

            var list = await _userService.GetAllUsersList();

            var result = new List<UserDTO>();
            foreach (var user in list)
            {
                List<string> roles = await _userService.GetUserRoles(user);


                UserDTO current = new UserDTO(user.FirstName, user.LastName, user.Address, user.Country, user.TelephoneNumber, user.Email, roles, user.ProfilePictureSrc);
                // this sadly cannot be used, as we are safeguarding e-mail with a private setter
                // we should not, and musn't allow email to be changeable. If allowed, it will cause chaos
                //UserDTO current = new UserDTO()
                //{
                //    Country = user.Country,
                //    Address = user.Address,
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    ProfilePicture = user.ProfilePictureSrc,
                //    Roles = roles,
                //    TelephoneNumber = user.TelephoneNumber
                //};

                result.Add(current);
            }
            return Ok(JsonSerializer.Serialize(result));
        }
        
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO user)
        {
            // todo : here


            Console.WriteLine(user);
            // 
            var result = await _userService.UpdateUser(user, false);



            return Ok(result);
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userService.GetRoles();
            var content = JsonSerializer.Serialize(roles);
            return Ok(content);
        }
    }
}
