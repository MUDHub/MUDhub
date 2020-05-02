using System;
using MUDhub.Server.ApiModels.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MUDhub.Server.Controllers;
using Xunit;
using Xunit.Sdk;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;

namespace MUDhub.Server.Tests
{
    public class AuthControllerTests : IDisposable
    {

        private readonly IUserManager _userManager;
        private readonly ILoginService _loginService;
        private readonly MudDbContext _context;
        private readonly User _user;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_AuthController")
                .Options;
            _context = new MudDbContext(options, true);
            var emailMock = Mock.Of<IEmailService>();
            //UserManager userManager = new UserManager(_context, emailMock);
            _user = new User("1")
            {
                Role = Roles.Master,
                Name = "Max",
                Lastname = "Mustermann",
                Email = "MAX@MUSTERMANN.DE",
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234"),
                PasswordResetKey = "ResetMax"
            };
            _context.Users.Add(_user);
            _context.SaveChanges();
            //_userManager = userManager;

        }

        public void Dispose()
        {
            _context.Users.Remove(_user);
            _context.SaveChanges();
            _context.Dispose();
        }

        [Fact]
        public async Task LoginAsync_ReturnOk()
        {
            var request = new LoginRequest()
            {
                Password = "admin",
                Email = "Admin@mudhub.de"
            };
            var authController = new AuthController(_loginService, _userManager);
        }
    }
}
