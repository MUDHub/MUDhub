using System;
using System.Collections.Generic;
using System.Text;
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
            var manager = new UserManager();
            Assert.True(manager.IsUserInRole("2", Roles.Master));
        }
    }
}
