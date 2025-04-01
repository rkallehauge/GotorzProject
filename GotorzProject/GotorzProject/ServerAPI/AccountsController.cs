using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GotorzProject.Shared;
using GotorzProject.Model;


namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<CustomUser> _userManager;

        public AccountsController(UserManager<CustomUser> userManager)
        {
            _userManager = userManager;
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

            return Ok(new RegisterResult { Successful = true });
        }

        [HttpPost("Employee")]
        public async Task<IActionResult> EmployeeCreation([FromBody] EmployeeRegisterModel model)
        {
            RegisterModel registerModel = (RegisterModel)model;
            var result = await Post(registerModel);

            return Ok(new RegisterResult { Successful = true });
        }
    }
}
