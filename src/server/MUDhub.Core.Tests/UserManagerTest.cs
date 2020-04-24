using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using MUDhub.Core.Services;
using MUDhub.Core.Services.Models;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class UserManagerTest
    {
        [Fact]
        public void CheckForAddingUserManagmentServices()
        {

        }

        [Fact]
        public async Task Test_IsUserInRoleAsync_ReturnTrue()
        {
            var userManager = await AddTestingDataAsync();
            Assert.True(await userManager.IsUserInRoleAsync("1", Roles.Master));
        }

        [Fact]
        public async Task Test_IsUserInRoleAsync_ReturnFalse()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.IsUserInRoleAsync("2", Roles.Master));
        }

        [Fact]
        public async Task Test_AddRoleToUserAsync_ReturnFalseBecauseNull()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.AddRoleToUserAsync("2", Roles.Master));
        }
        [Fact]
        public async Task Test_AddRoleToUserAsync_ReturnFalseBecauseRole()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.AddRoleToUserAsync("1", Roles.Master));
        }
        [Fact]
        public async Task Test_AddRoleToUserAsync_ReturnTrue()
        {
            var userManager = await AddTestingDataAsync();
            Assert.True(await userManager.AddRoleToUserAsync("1", Roles.Admin));
        }

        [Fact]
        public async Task Test_RemoveRoleFromUserAsync_ReturnFalseBecauseNull()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.RemoveRoleFromUserAsync("2", Roles.Master));
        }


        [Fact]
        public async Task Test_RemoveRoleFromUserAsync_ReturnFalseBecauseRole()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.RemoveRoleFromUserAsync("1", Roles.Admin));
        }
        [Fact]
        public async Task Test_RemoveRoleFromUserAsync_ReturnTrue()
        {
            var userManager = await AddTestingDataAsync();
            Assert.True(await userManager.RemoveRoleFromUserAsync("1", Roles.Master));
        }

        [Fact]
        public async Task Test_UpdatePasswortAsync_ReturnFalseBecauseNull()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortAsync("2", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task Test_UpdatePasswortAsync_ReturnFalseBecauseEqual()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortAsync("1", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task Test_UpdatePasswortAsync_ReturnTrue()
        {
            var userManager = await AddTestingDataAsync();
            Assert.True(await userManager.UpdatePasswortAsync("1", "PW1234", "1234PW"));
        }

        [Fact]
        public async Task Test_RemoveUserAsync_ReturnFalseBecauseNull()
        {
            var userManager = await AddTestingDataAsync();
            Assert.False(await userManager.RemoveUserAsync("2"));
        }
        [Fact]
        public async Task Test_RemoveUserAsync_ReturnTrue()
        {
            var userManager = await AddTestingDataAsync();
            Assert.True(await userManager.RemoveUserAsync("1"));
        }

        public static IEnumerable<object[]> CreateRegistrationArgs()
        {
            yield return new object[]{ new RegistrationArgs()
            {
                Name = null,
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = null,
                Email = "max@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = null,
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = null
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = string.Empty,
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = string.Empty,
                Email = "max@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = string.Empty,
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = string.Empty
            } };
        }

        [Theory]
        [MemberData(nameof(CreateRegistrationArgs))]
        public async Task Test_RegisterUserAsync_ReturnFalseBecauseNullOrEmpty(RegistrationArgs registrationArgs)
        {
            var userManager = await AddTestingDataAsync();
            
            RegisterResult registerResult = await userManager.RegisterUserAsync(registrationArgs);
            Assert.False(registerResult.Succeeded);
        }
        [Fact]
        public async Task Test_RegisterUserAsync_ReturnFalseUserExist()
        {
            var userManager = await AddTestingDataAsync();
            var regiArgs = new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = "Test1234"
            };
            RegisterResult registerResult = await userManager.RegisterUserAsync(regiArgs);
            Assert.True(registerResult.Succeeded);
        }
        [Fact]
        public async Task Test_RegisterUserAsync_ReturnTrue()
        {
            var userManager = await AddTestingDataAsync();
            var regiArgs = new RegistrationArgs()
            {
                Name = "Max",
                Lastname = "Mustermann",
                Email = "max@test.de",
                Password = "Test1234"
            };
            RegisterResult registerResult = await userManager.RegisterUserAsync(regiArgs);
            Assert.False(registerResult.Succeeded);
        }









        private static async Task<UserManager> AddTestingDataAsync()
        {
            var context = CreateInMemoryDbContext();
            var emailMock = Mock.Of<IEmailService>();
            var userManager = new UserManager(context, emailMock);
            context.Users.Add(new User("1")
            {
                Role = Roles.Master,
                Name = "Max",
                Lastname = "Mustermann",
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234")
            });
            await context.SaveChangesAsync();
            return userManager;
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
