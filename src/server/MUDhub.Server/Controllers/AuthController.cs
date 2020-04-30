using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services.Models;
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
                    Succeeded = true,
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
        public async Task RegisterAsync()
        {
        }

        [HttpGet("reset")]//Todo: maybe http post, need discussion
        public async Task RequestResetPasswordAsync()
        {

        }

        [HttpPost("reset")]//Todo: maybe http post, need discussion
        public async Task ResetPasswordAsync()
        {

        }
    }
}
