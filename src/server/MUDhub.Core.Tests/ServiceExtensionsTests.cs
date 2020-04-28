﻿using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Services;
using MUDhub.Core.Abstracts;

namespace MUDhub.Core.Tests
{
    public class ServiceExtensionsTests
    {
        [Fact]
        public void CheckForAddingUserManagementServices()
        {
            var collection = new ServiceCollection();
            collection.AddUserManagment();
            Assert.Contains(collection, s => s.ImplementationType == typeof(UserManager) && s.ServiceType == typeof(IUserManager));
            Assert.Contains(collection, s => s.ImplementationType == typeof(LoginService) && s.ServiceType == typeof(ILoginService));
            Assert.Contains(collection, s => s.ImplementationType == typeof(EmailService) && s.ServiceType == typeof(IEmailService));
            Assert.Equal(3, collection.Count); //Checking for new Services
        }


        [Fact]
        public void CheckForAddingMudManagementServices()
        {
            var collection = new ServiceCollection();
            collection.AddMudGameManagment();
            Assert.Contains(collection, s => s.ImplementationType == typeof(MudManager) && s.ServiceType == typeof(IMudManager));
            Assert.Contains(collection, s => s.ImplementationType == typeof(GameService) && s.ServiceType == typeof(IGameService));
            Assert.Equal(2, collection.Count); //Checking for new Services
        }

    }
}
