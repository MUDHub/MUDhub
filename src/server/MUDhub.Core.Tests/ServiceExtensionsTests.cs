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
            Assert.Contains(collection, s => s.ImplementationType == typeof(UserManager) && s.ServiceType == typeof(IUserManager));
            Assert.Contains(collection, s => s.ImplementationType == typeof(LoginService) && s.ServiceType == typeof(ILoginService));
            Assert.Equal(2, collection.Count); //Checking for new Services
        }


        [Fact]
        public void CheckForAddingMudManagmentServices()
        {
            var collection = new ServiceCollection();
            collection.AddMudGameManagment();
            Assert.Contains(collection, s => s.ImplementationType == typeof(MudManager) && s.ServiceType == typeof(IMudManager));
            Assert.Contains(collection, s => s.ImplementationType == typeof(GameService) && s.ServiceType == typeof(IGameService));
            Assert.Equal(2, collection.Count); //Checking for new Services
        }

    }
}
