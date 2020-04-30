using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Server.ApiModels;

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
        public async Task<LoginResponse> LoginAsync([FromBody] LoginRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var result = await _loginService.LoginUserAsync(args.Email, args.Password)
                .ConfigureAwait(false);
            if (result.Succeeded)
            {
                return new LoginResponse()
                {
                    Firstname = result.User!.Name,
                    Lastname = result.User!.Lastname,
                    Token = result.Token
                };
            }
            else
            {
                return new LoginResponse()
                {
                    Statuscode = 400,
                    Errormessage = "Username or Password is false!"
                };
            }
        }

        [HttpPost("register")]
        public async Task<RegisterResponse> RegisterAsync([FromBody] RegisterRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var regiArgs = new RegistrationArgs()
            {
                Email = args.Email,
                Lastname = args.Lastname,
                Name = args.Name,
                Password = args.Password
            };

            var registerResult = await _userManager.RegisterUserAsync(regiArgs).ConfigureAwait(false);
            if (registerResult.Succeeded)
            {
                return new RegisterResponse();
            }
            else
            {
                return new RegisterResponse()
                {
                    Succeeded = false,
                    Statuscode = 400,
                    Errormessage = registerResult.UsernameAlreadyExists ? "Username already exist" : "Cannot register the User"
                };
            }
        }

        //Todo: maybe http post, need discussion
        [HttpGet("reset")]
        public async Task RequestResetPasswordAsync()
        {

        }

        //Todo: maybe http post, need discussion
        [HttpPost("reset")]
        public async Task ResetPasswordAsync()
        {

        }
    }
}
