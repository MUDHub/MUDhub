using MUDhub.Core.Configurations;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class LoginServiceTest : IDisposable
    {
        private readonly LoginService _loginService;
        private readonly UserManager _userManager;
        private readonly MudDbContext _context;
        private readonly User _user;

        public LoginServiceTest()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_LoginService")
                .Options;
            _context = new MudDbContext(options);
            var emailMock = Mock.Of<IEmailService>();
            var userManager = new UserManager(_context, emailMock);

            _loginService = new LoginService(_context, userManager, new ServerConfiguration());
            _user = new User("sdfsdf")
            {
                Role = Role.Master,
                Name = "Max",
                Lastname = "Mustermann",
                Email = "Max@Mustermann.de",
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234"),
                PasswordResetKey = "ResetMax"
            };
            _context.Users.Add(_user);
            _context.SaveChanges();
            _userManager = userManager;

        }

        public void Dispose()
        {
            _context.Users.Remove(_user);
            _context.SaveChanges();
            _context.Dispose();
        }

        [Fact]
        public async Task LoginUserSuccessfully()
        {
            var email = "Max@Mustermann.de";
            var password = "PW1234";
            var loginResult = await _loginService.LoginUserAsync(email, password);
            Assert.True(loginResult.Succeeded);
        }

        [Fact]
        public async Task LoginUserFailedBecauseEmailNotExist()
        {
            var email = "Tobi@Wurst.de";
            var password = "PW1234";
            var loginResult = await _loginService.LoginUserAsync(email, password);
            Assert.False(loginResult.Succeeded);
        }

        [Fact]
        public async Task LoginUserFailedBecausePasswordIsFalse()
        {
            var email = "Max@Mustermann.de";
            var password = "1234PW";
            var loginResult = await _loginService.LoginUserAsync(email, password);
            Assert.False(loginResult.Succeeded);
        }
    }
}
