using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GotorzProject.Shared;
using GotorzProject.Model;
using GotorzProject.Service;


namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<CustomUser> _userManager;
        private readonly IUserAdminstration _userAdminstration;

        public AccountsController(UserManager<CustomUser> userManager, IUserAdminstration userAdmin)
        {
            _userManager = userManager;
            _userAdminstration = userAdmin;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new CustomUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            // Automagically set first user to be admin, so as we don't have to manually add a an admin in database directly
            // This code will also run if all other users somehow get deleted
            if(_userManager.Users.Count() == 1)
            {
                Console.WriteLine($"Adding newly created user {newUser.Email} as admin.");
                await _userManager.AddToRoleAsync(newUser, "Admin"); // lq
            }

            return Ok(new RegisterResult { Successful = true });
        }

        [HttpPost("Employee")]
        public async Task<IActionResult> EmployeeCreation([FromBody] EmployeeRegisterModel model)
        {
            RegisterModel registerModel = (RegisterModel)model;
            var result = await Post(registerModel);

            return Ok(new RegisterResult { Successful = true });
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            Console.WriteLine(_userManager.Users.Count());
            return Ok();
        }
    }
}
