using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using MUDhub.Core.Abstracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MUDhub.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace MUDhub.Core.Tests
{
    public class ServiceExtensionsTests
    {
        [Fact]
        public void CheckForAddingUserManagmentServices()
        {
            var collection = new ServiceCollection();
            collection.AddUserManagment();
            Assert.Contains(collection, s => s.ImplementationType == typeof(UserManager));
            Assert.Contains(collection, s => s.ImplementationType == typeof(LoginService));
            Assert.Equal(2, collection.Count); //Checking for new Services
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
