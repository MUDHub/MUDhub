using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using MUDhub.Server.ApiModels.Auth;

namespace MUDhub.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IUserManager _userManager;

        public AuthController(ILoginService loginService, IUserManager userManager)
        {
            _loginService = loginService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var result = await _loginService.LoginUserAsync(args.Email, args.Password)
                .ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Ok(new LoginResponse()
                {
                    Token = result.Token,
                    User = new UserApiModel(result.User!)
                });
            }

            return BadRequest(new LoginResponse()
            {
                Errormessage = "Username or Password is wrong!"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            

            var registerResult = await _userManager.RegisterUserAsync(RegisterRequest.ConvertFromRequest(args)).ConfigureAwait(false);
            if (registerResult.Succeeded)
            {
                return Ok(new RegisterResponse()
                {
                    User = UserApiModel.CreateFromUser(registerResult.User!)
                });
            }

            return BadRequest(new RegisterResponse()
            {
                Succeeded = false,
                Errormessage = registerResult.UsernameAlreadyExists ? "Username already exist" : "Cannot register the User"
            });
        }

        [HttpGet("reset")]
        public async Task RequestResetPasswordAsync([FromQuery] string email)
        {
            await _userManager.GeneratePasswortResetAsync(email).ConfigureAwait(false);
        }

        [HttpPost("reset")]
        public async Task<ActionResult<LoginResponse>> ResetPasswordAsync([FromBody] ResetPasswordRequest args)
        {
            var isResetSuccessful = await _userManager.UpdatePasswortFromResetAsync(args.PasswordResetKey, args.NewPasword)
                .ConfigureAwait(false);
            if (isResetSuccessful)
            {
                return Ok(new ResetPasswordResponse());
            }

            return BadRequest(new ResetPasswordResponse()
            {
                Succeeded = false,
                Errormessage = "Password could not be reseted. => Maybe ResetKey is wrong or new Password is equal old Password."
            });
        }
    }
}
