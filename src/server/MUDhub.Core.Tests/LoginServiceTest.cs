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
    public class LoginServiceTest
    {
        
        [Fact]
        public async Task LoginUserSuccessfully()
        {
            var loginService = CreateInMemoryDataBaseWithTestingDataAsync();
            var email = "Max@Mustermann.de";
            var password = "PW1234";
            var loginResult = await loginService.LoginUserAsync(email, password);
            Assert.True(loginResult.Succeeded);
        }

        [Fact]
        public async Task LoginUserFailedBecauseEmailNotExist()
        {
            var loginService = CreateInMemoryDataBaseWithTestingDataAsync();
            var email = "Tobi@Wurst.de";
            var password = "PW1234";
            var loginResult = await loginService.LoginUserAsync(email, password);
            Assert.False(loginResult.Succeeded);
        }

        [Fact]
        public async Task LoginUserFailedBecausePasswordIsFalse()
        {
            var loginService = CreateInMemoryDataBaseWithTestingDataAsync();
            var email = "Max@Mustermann.de";
            var password = "1234PW";
            var loginResult = await loginService.LoginUserAsync(email, password);
            Assert.False(loginResult.Succeeded);
        }





        private static LoginService CreateInMemoryDataBaseWithTestingDataAsync()
        {
            var context = CreateInMemoryDbContext();
            var emailMock = Mock.Of<IEmailService>();
            
            var userManager = new UserManager(context, emailMock);

            var loginService = new LoginService(context, userManager, new ServerConfiguration());

            context.Users.Add(new User("1")
            {
                Role = Roles.Master,
                Name = "Max",
                Lastname = "Mustermann",
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234"),
                PasswordResetKey = "ResetMax",
                Email = "Max@Mustermann.de"
            });
            context.SaveChanges();
            return loginService;
        }
        private static MudDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase")
                .Options;

            return new MudDbContext(options);
        }

    }
}
