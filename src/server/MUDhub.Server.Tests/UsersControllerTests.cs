using Moq;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models;
using MUDhub.Server.ApiModels.Auth;
using MUDhub.Server.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MUDhub.Server.Tests
{
    public class UsersControllerTests
    {

        [Fact]
        public async Task LoginAsync_ReturnOk()
        {

            //var userManagerMock = new Mock<IUserManager>();
            //userManagerMock
            //    .Setup(um => um.RemoveUserAsync("1234"))
            //    .Returns(Task.FromResult(true));
            //var um = userManagerMock.Object;

            //var usersController = new UsersController(MudDbContextMocker.MockDbContext(), um);
            //var result = usersController.DeleteUser("1234").Result;

            //Assert.IsType<OkObjectResult>(result);
        }
    }
}
