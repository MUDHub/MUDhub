﻿using MUDhub.Server.ApiModels.Auth;
using System.Threading.Tasks;
using MUDhub.Server.Controllers;
using Xunit;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using Moq;
using MUDhub.Core.Models;
using MUDhub.Core.Abstracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace MUDhub.Server.Tests
{
    public class AuthControllerTests
    {

        [Fact]
        public async Task LoginAsync_ReturnOk()
        {

            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock
                .Setup(ls => ls.LoginUserAsync("Test", "test"))
                .Returns(Task.FromResult(new LoginResult(true, "token", new User())));
            var ls = loginServiceMock.Object;

            var authController = new AuthController(ls, Mock.Of<IUserManager>());
            var result = authController.LoginAsync(new LoginRequest() { Email = "Test", Password = "test" }).Result;

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RegisterAsync_ReturnOk()
        {
            var args = new RegisterRequest
            {
                Email = "Test",
                Firstname = "sadfsd ",
                Lastname = "sd fs",
                Password = "s df"
            };

            var ls = Mock.Of<ILoginService>();
            var umMock = new Mock<IUserManager>();
            umMock.Setup(um => um.RegisterUserAsync(RegisterRequest.ConvertFromRequest(args)))
                            .Returns(Task.FromResult(new RegisterResult(true, false, new User())));


            var authController = new AuthController(ls, umMock.Object);
            var result = await authController.RegisterAsync(args);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ResetPasswordAsync_ReturnOk()
        {
            //var args = new ResetPasswordRequest
            //{
            //    NewPasword = "saferPassword",
            //    PasswordResetKey = "crazyResetKey"
            //};

            //var ls = Mock.Of<ILoginService>();
            //var umMock = new Mock<IUserManager>();
            //umMock.Setup(um => um.UpdatePasswortFromResetAsync(args.NewPasword, args.PasswordResetKey))
            //    .Returns(Task.FromResult(true));


            //var authController = new AuthController(ls, umMock.Object);
            //var result = await authController.ResetPasswordAsync(args);

            //Assert.IsType<OkObjectResult>(result);
        }
    }
}
