using MUDhub.Core.Configurations;
using MUDhub.Core.Services;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class LoginServiceTest : IDisposable
    {
        private readonly LoginService _loginService;
        private readonly MudDbContext _context;
        private readonly User _user;

        public LoginServiceTest()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_LoginService")
                .Options;
            _context = new MudDbContext(options,useNotInUnitests: false);

            _loginService = new LoginService(_context, new ServerConfiguration() { TokenSecret = "sdsdfsdfn 3b4t 45 tb45k n45 ö- zh56 zn56 jb34 " });
            _user = new User("sdfsdf")
            {
                Role = Roles.Master,
                Name = "Max",
                Lastname = "Mustermann",
                Email = "Max@Mustermann.de",
                NormalizedEmail = "MAX@MUSTERMANN.DE",
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234"),
                PasswordResetKey = "ResetMax"
            };
            _context.Users.Add(_user);
            _context.SaveChanges();

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
