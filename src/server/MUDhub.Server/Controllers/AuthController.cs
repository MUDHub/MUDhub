using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Server.ApiModels;

namespace MUDhub.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task LoginAsync([FromBody]LoginArgs args)
        {
            _loginService.LoginAsync()
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
