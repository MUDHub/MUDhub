using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.True(await userManager.IsUserInRoleAsync("1", Roles.Master));
        }

        [Fact]
        public async Task Test_IsUserInRoleAsync_ReturnFalse()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.IsUserInRoleAsync("2", Roles.Master));
        }

        [Fact]
        public async Task Test_AddRoleToUserAsync_ReturnFalseBecauseNull()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.AddRoleToUserAsync("2", Roles.Master));
        }
        [Fact]
        public async Task Test_AddRoleToUserAsync_ReturnFalseBecauseRole()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.AddRoleToUserAsync("1", Roles.Master));
        }
        [Fact]
        public async Task Test_AddRoleToUserAsync_ReturnTrue()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.True(await userManager.AddRoleToUserAsync("1", Roles.Admin));
        }

        [Fact]
        public async Task Test_RemoveRoleFromUserAsync_ReturnFalseBecauseNull()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.RemoveRoleFromUserAsync("2", Roles.Master));
        }
        [Fact]
        public async Task Test_RemoveRoleFromUserAsync_ReturnFalseBecauseRole()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.RemoveRoleFromUserAsync("1", Roles.Admin));
        }
        [Fact]
        public async Task Test_RemoveRoleFromUserAsync_ReturnTrue()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.True(await userManager.RemoveRoleFromUserAsync("1", Roles.Master));
        }

        [Fact]
        public async Task Test_UpdatePasswortAsync_ReturnFalseBecauseNull()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.UpdatePasswortAsync("2", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task Test_UpdatePasswortAsync_ReturnFalseBecauseEqual()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.UpdatePasswortAsync("1", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task Test_UpdatePasswortAsync_ReturnTrue()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.True(await userManager.UpdatePasswortAsync("1", "PW1234", "1234PW"));
        }

        [Fact]
        public async Task Test_RemoveUserAsync_ReturnFalseBecauseNull()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.RemoveUserAsync("2"));
        }
        [Fact]
        public async Task Test_RemoveUserAsync_ReturnTrue()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.True(await userManager.RemoveUserAsync("1"));
        }

        [Fact]
        public async Task Test_RegisterUserAsync_ReturnFalseBecauseNullOrEmpty()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            var regiArgs = new RegistrationArgs()
            {

            };
            RegisterResult registerResult = await userManager.RegisterUserAsync(regiArgs);
            Assert.False(registerResult.Succeeded);
        }
        [Fact]
        public async Task Test_RegisterUserAsync_ReturnTrue()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            var regiArgs = new RegistrationArgs()
            {
                Name = "Max",
                Lastname = "Mustermann",
                Email = "max@test.de",
                Password = "Test1234"
            };
            RegisterResult registerResult = await userManager.RegisterUserAsync(regiArgs);
            Assert.True(registerResult.Succeeded);
        }











        private static async Task AddTestingData(MudDbContext context)
        {
            var userManager = new UserManager(context);
            context.Users.Add(new User("1")
            {
                Role = Roles.Master,
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234")
            });
            await context.SaveChangesAsync();
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
