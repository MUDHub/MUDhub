using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task IsUserInRoleAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.True(await userManager.IsUserInRoleAsync("1", Roles.Master));
        }

        [Fact]
        public async Task IsUserInRoleAsync_ReturnFalse()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.IsUserInRoleAsync("2", Roles.Master));
        }

        [Fact]
        public async Task AddRoleToUserAsync_ReturnFalseBecauseNull()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.AddRoleToUserAsync("2", Roles.Master));
        }
        [Fact]
        public async Task AddRoleToUserAsync_ReturnFalseBecauseRole()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.AddRoleToUserAsync("1", Roles.Master));
        }
        [Fact]
        public async Task AddRoleToUserAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.True(await userManager.AddRoleToUserAsync("1", Roles.Admin));
        }

        [Fact]
        public async Task RemoveRoleFromUserAsync_ReturnFalseBecauseNull()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.RemoveRoleFromUserAsync("2", Roles.Master));
        }


        [Fact]
        public async Task RemoveRoleFromUserAsync_ReturnFalseBecauseRole()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.RemoveRoleFromUserAsync("1", Roles.Admin));
        }
        [Fact]
        public async Task RemoveRoleFromUserAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.True(await userManager.RemoveRoleFromUserAsync("1", Roles.Master));
        }

        [Fact]
        public async Task UpdatePasswortAsync_ReturnFalseBecauseNull()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortAsync("2", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortAsync_ReturnFalseBecauseEqual()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortAsync("1", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortAsync_ReturnFalseBecauseOldPasswordIsWrong()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortAsync("1", "PW4321", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.True(await userManager.UpdatePasswortAsync("1", "PW1234", "1234PW"));
        }

        [Fact]
        public async Task UpdatePasswortFromResetAsync_ReturnFalseBecauseNull()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortFromResetAsync("2", "PW1234"));
        }

        [Fact]
        public async Task UpdatePasswortFromResetAsync_ReturnFalseBecauseEqual()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.UpdatePasswortFromResetAsync("ResetMax", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortFromResetAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.True(await userManager.UpdatePasswortFromResetAsync("ResetMax", "1234PW"));
        }



        [Fact]
        public async Task RemoveUserAsync_ReturnFalseBecauseNull()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.False(await userManager.RemoveUserAsync("2"));
        }
        [Fact]
        public async Task RemoveUserAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
            Assert.True(await userManager.RemoveUserAsync("1"));
        }

        public static IEnumerable<object[]> CreateRegistrationArgs()
        {
            yield return new object[]{ new RegistrationArgs()
            {
                Name = null!,
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = null!,
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = null!
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
            yield return new object[]{ new RegistrationArgs()
            {
                Name = " ",
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = " ",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationArgs()
            {
                Name = "Tobi",
                Lastname = "Kartoffel",
                Email = "max@test.de",
                Password = " "
            } };
        }

        [Theory]
        [MemberData(nameof(CreateRegistrationArgs))]
        public async Task RegisterUserAsync_ReturnFalseBecauseNullOrEmpty(RegistrationArgs registrationArgs)
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();

            RegisterResult registerResult = await userManager.RegisterUserAsync(registrationArgs);
            Assert.False(registerResult.Succeeded);
        }
        [Fact]
        public async Task RegisterUserAsync_ReturnFalseUserExist()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
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
        public async Task RegisterUserAsync_ReturnTrue()
        {
            var userManager = CreateInMemoryDataBaseWithTestingDataAsync();
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

        private static UserManager CreateInMemoryDataBaseWithTestingDataAsync()
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
            context.SaveChanges();
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
