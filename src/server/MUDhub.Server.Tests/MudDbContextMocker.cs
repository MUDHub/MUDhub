using Microsoft.EntityFrameworkCore;
using Moq;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDhub.Server.Tests
{
    public static class MudDbContextMocker
    {
        public static MudDbContext MockDbContext()
        {
            var usersMock = CreateDbSetMock(GetFakeListofUsers());
            var mockDbContext = new Mock<MudDbContext>();
            mockDbContext.Setup(x => x.Users).Returns(usersMock.Object);
            return mockDbContext.Object;
        }


        private static IEnumerable<User> GetFakeListofUsers()
        {
            yield return new User("1")
            {
                Role = Roles.Master,
                Name = "Max",
                Lastname = "Mustermann",
                Email = "MAX@MUSTERMANN.DE",
                PasswordHash = UserHelpers.CreatePasswordHash("PW1234"),
                PasswordResetKey = "ResetMax"
            };
        }


        private static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());
            return dbSetMock;
        }
    }
}
