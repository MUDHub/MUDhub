using Microsoft.EntityFrameworkCore;
using Moq;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Helper;
using MUDhub.Core.Models.Users;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class UserManagerTest : IDisposable
    {
        private readonly UserManager _userManager;
        private readonly MudDbContext _context;
        private readonly User _user;

        public UserManagerTest()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_UserManagment")
                .Options;
            _context = new MudDbContext(options);
            var emailMock = Mock.Of<IEmailService>();
            var userManager = new UserManager(_context, emailMock);
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
            _userManager = userManager;

        }

        public void Dispose()
        {
            _context.Users.Remove(_user);
            _context.SaveChanges();
            _context.Dispose();
        }


        [Fact]
        public void CheckForAddingUserManagmentServices()
        {

        }

        [Fact]
        public async Task IsUserInRoleAsync_ReturnTrue()
        {
            Assert.True(await _userManager.IsUserInRoleAsync(_user.Id, Roles.Master));

        }

        [Fact]
        public async Task IsUserInRoleAsync_ReturnFalse()
        {
            Assert.False(await _userManager.IsUserInRoleAsync("2", Roles.Master));
        }

        [Fact]
        public async Task AddRoleToUserAsync_ReturnFalseBecauseNull()
        {

            Assert.False(await _userManager.AddRoleToUserAsync("2", Roles.Master));
        }
        [Fact]
        public async Task AddRoleToUserAsync_ReturnFalseBecauseRole()
        {

            Assert.False(await _userManager.AddRoleToUserAsync("1", Roles.Master));
        }
        [Fact]
        public async Task AddRoleToUserAsync_ReturnTrue()
        {

            Assert.True(await _userManager.AddRoleToUserAsync("1", Roles.Admin));
        }

        [Fact]
        public async Task RemoveRoleFromUserAsync_ReturnFalseBecauseNull()
        {

            Assert.False(await _userManager.RemoveRoleFromUserAsync("2", Roles.Master));
        }


        [Fact]
        public async Task RemoveRoleFromUserAsync_ReturnFalseBecauseRole()
        {

            Assert.False(await _userManager.RemoveRoleFromUserAsync("1", Roles.Admin));
        }
        [Fact]
        public async Task RemoveRoleFromUserAsync_ReturnTrue()
        {

            Assert.True(await _userManager.RemoveRoleFromUserAsync("1", Roles.Master));
        }

        [Fact]
        public async Task UpdatePasswortAsync_ReturnFalseBecauseNull()
        {

            Assert.False(await _userManager.UpdatePasswortAsync("2", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortAsync_ReturnFalseBecauseEqual()
        {

            Assert.False(await _userManager.UpdatePasswortAsync("1", "PW1234", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortAsync_ReturnFalseBecauseOldPasswordIsWrong()
        {

            Assert.False(await _userManager.UpdatePasswortAsync("1", "PW4321", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortAsync_ReturnTrue()
        {

            Assert.True(await _userManager.UpdatePasswortAsync("1", "PW1234", "1234PW"));
        }

        [Fact]
        public async Task UpdatePasswortFromResetAsync_ReturnFalseBecauseNull()
        {

            Assert.False(await _userManager.UpdatePasswortFromResetAsync("2", "PW1234"));
        }

        [Fact]
        public async Task UpdatePasswortFromResetAsync_ReturnFalseBecauseEqual()
        {

            Assert.False(await _userManager.UpdatePasswortFromResetAsync("ResetMax", "PW1234"));
        }
        [Fact]
        public async Task UpdatePasswortFromResetAsync_ReturnTrue()
        {
            var user = new User()
            {
                PasswordHash = UserHelpers.CreatePasswordHash("1234"),
                PasswordResetKey = "ResetMax2123"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            Assert.True(await _userManager.UpdatePasswortFromResetAsync("ResetMax2123", "1234PW"));
        }



        [Fact]
        public async Task RemoveUserAsync_ReturnFalseBecauseNull()
        {

            Assert.False(await _userManager.RemoveUserAsync("2"));
        }
        [Fact]
        public async Task RemoveUserAsync_ReturnTrue()
        {
            _context.Users.Add(new User("12"));
            await _context.SaveChangesAsync();
            Assert.True(await _userManager.RemoveUserAsync("12"));
        }

        public static IEnumerable<object[]> CreateRegistrationArgs()
        {
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = null!,
                Lastname = "Kartoffel",
                Email = "max1@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = null!,
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = "max2@test.de",
                Password = null!
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = string.Empty,
                Lastname = "Kartoffel",
                Email = "max3@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = string.Empty,
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = "max4@test.de",
                Password = string.Empty
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = " ",
                Lastname = "Kartoffel",
                Email = "max5@test.de",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = " ",
                Password = "Test1234"
            } };
            yield return new object[]{ new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = "max6@test.de",
                Password = " "
            } };
        }

        [Theory]
        [MemberData(nameof(CreateRegistrationArgs))]
        public async Task RegisterUserAsync_ReturnFalseBecauseNullOrEmpty(RegistrationUserArgs RegistrationUserArgs)
        {


            RegisterResult registerResult = await _userManager.RegisterUserAsync(RegistrationUserArgs);
            Assert.False(registerResult.Success);
        }
        [Fact]
        public async Task RegisterUserAsync_ReturnFalseUserExist()
        {

            var regiArgs = new RegistrationUserArgs()
            {
                Firstname = "Tobi",
                Lastname = "Kartoffel",
                Email = "max@musterman.de",
                Password = "Test1234"
            };
            RegisterResult registerResult = await _userManager.RegisterUserAsync(regiArgs);
            Assert.True(registerResult.Success);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnTrue()
        {

            var regiArgs = new RegistrationUserArgs()
            {
                Firstname = "Max",
                Lastname = "Mustermann",
                Email = "max8@test.de",
                Password = "Test1234"
            };
            RegisterResult registerResult = await _userManager.RegisterUserAsync(regiArgs);
            Assert.True(registerResult.Success);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnTrue()
        {

            var updateArgs = new UpdateUserArgs()
            {
                Firstname = "Max",
                Lastname = "Mustermann"
            };
            var testResult = await _userManager.UpdateUserAsync("1", updateArgs);
            Assert.NotNull(testResult);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnFalseUserNotFound()
        {

            var updateArgs = new UpdateUserArgs()
            {
                Firstname = "Max",
                Lastname = "Mustermann"
            };
            var testResult = await _userManager.UpdateUserAsync("2", updateArgs);
            Assert.Null(testResult);
        }


    }
}
