using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GotorzProject.Shared;
using GotorzProject.Model;
using GotorzProject.Service.System;


namespace GotorzProject.ServerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<CustomUser> _userManager;
        private readonly DatabaseLogger _databaseLogger;


        public AccountsController(UserManager<CustomUser> userManager, DatabaseLogger databaseLogger)
        {
            _userManager = userManager;
            _databaseLogger = databaseLogger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new CustomUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                _databaseLogger.LogInformation($"Registration attempt failed.");
                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            _databaseLogger.LogInformation($"User successfully registered.");
            return Ok(new RegisterResult { Successful = true });
        }

        [HttpPost("Employee")]
        public async Task<IActionResult> EmployeeCreation([FromBody] EmployeeRegisterModel model)
        {
            RegisterModel registerModel = (RegisterModel)model;
            var result = await Post(registerModel);
            _databaseLogger.LogInformation($"Employee successfully registered");
            return Ok(new RegisterResult { Successful = true });
        }
    }
}
