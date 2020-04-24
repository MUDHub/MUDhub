using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using MUDhub.Core.Services;
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
        public async Task TestIsEmailExist()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.True(await userManager.IsUserInRoleAsync("1", Roles.Master));
        }

        [Fact]
        public async Task TestIsEmailExist2()
        {
            var context = CreateInMemoryDbContext();
            await AddTestingData(context);
            var userManager = new UserManager(context);
            Assert.False(await userManager.IsUserInRoleAsync("2", Roles.Master));
        }

        private static async Task AddTestingData(MudDbContext context)
        {
            context.Users.Add(new User("1") { Role = Roles.Master });
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
