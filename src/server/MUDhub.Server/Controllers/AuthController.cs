﻿using System;
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
        public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest args)
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
                    User = new UserApiModel
                    {
                        Email = result.User!.Email,
                        Id = result.User.Id,
                        FirstName = result.User.Name,
                        LastName = result.User.Lastname,
                        Roles = UserHelpers.ConvertRoleToList(result.User.Role),                        
                    }
                });
            }

            return BadRequest(new LoginResponse()
            {
                Statuscode = StatusCodes.Status400BadRequest,
                Errormessage = "Username or Password is false!"
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> RegisterAsync([FromBody] RegisterRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var regiArgs = new UserModelArgs()
            {
                Email = args.Email,
                Lastname = args.Lastname,
                Name = args.Name,
                Password = args.Password
            };

            var registerResult = await _userManager.RegisterUserAsync(regiArgs).ConfigureAwait(false);
            if (registerResult.Succeeded)
            {
                return Ok(new RegisterResponse());
            }

            return BadRequest(new RegisterResponse()
            {
                Succeeded = false,
                Statuscode = StatusCodes.Status400BadRequest,
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
                Statuscode = StatusCodes.Status400BadRequest,
                Errormessage = "Password could not be reseted. => Maybe ResetKey is wrong or new Password is equal old Password."
            });
        }
    }
}
