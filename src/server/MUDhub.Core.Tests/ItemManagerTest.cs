using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts.Models.Inventories;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Users;
using MUDhub.Core.Services;
using System.Threading.Tasks;
using System;
using Xunit;
using System.Linq;

namespace MUDhub.Core.Tests
{
    public class ItemManagerTest : IDisposable
    {
        private ItemManager _itemManager;
        private MudDbContext _context;

        private User _user1, _user2;
        private MudGame _mudGame1;
        private Item _item1;

        private ItemArgs _createArgs, _updateArgs;

        public ItemManagerTest()
        {
            InitializeTestDb();
        }

        public void Dispose()
        {
            _context.Users.Remove(_user2);
            _context.MudGames.Remove(_mudGame1);
            _context.SaveChanges();
            _context.Dispose();
        }

        //*********************************************************//

        [Fact]
        public async Task CreateItemAsync_ReturnFalse_UserNull()
        {
            var result = await _itemManager.CreateItemAsync("99", "1", _createArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemAsync_ReturnFalse_MudNull()
        {
            var result = await _itemManager.CreateItemAsync("1", "99", _createArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _itemManager.CreateItemAsync("2", "1", _createArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemAsync_ReturnTrue()
        {
            var result = await _itemManager.CreateItemAsync("1", "1", _createArgs);
            Assert.True(result.Success);

            var newItem = _context.Items.FirstOrDefault(a => a.Name.Equals("Create Item"));
            Assert.True(newItem.Description.Equals("Create Beschreibung Item"));
        }

        //*********************************************************//

        [Fact]
        public async Task UpdateItemAsync_ReturnFalse_UserNull()
        {
            var result = await _itemManager.UpdateItemAsync("99", "1", _updateArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateItemAsync_ReturnFalse_ItemNull()
        {
            var result = await _itemManager.UpdateItemAsync("1", "99", _updateArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateItemAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _itemManager.UpdateItemAsync("2", "1", _updateArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateItemAsync_ReturnTrue()
        {
            var result = await _itemManager.UpdateItemAsync("1", "1", _updateArgs);
            Assert.True(result.Success);

            var newItem = _context.Items.FirstOrDefault(a => a.Name.Equals("Update Item"));
            Assert.True(newItem.Description.Equals("Update Beschreibung Item"));
        }

        //*********************************************************//

        [Fact]
        public async Task DeleteItemAsync_ReturnFalse_UserNull()
        {
            var result = await _itemManager.DeleteItemAsync("99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task DeleteItemAsync_ReturnFalse_ItemNull()
        {
            var result = await _itemManager.DeleteItemAsync("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task DeleteItemAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _itemManager.DeleteItemAsync("2", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task DeleteItemAsync_ReturnTrue()
        {
            var result = await _itemManager.DeleteItemAsync("1", "1");
            Assert.True(result.Success);

            var oldItem = _context.Items.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(oldItem is null);
        }

        //*********************************************************//

        private void InitializeTestDb()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_ItemManager")
                .Options;
            _context = new MudDbContext(options);
            _itemManager = new ItemManager(_context);

            _user1 = new User("1")
            {
                Name = "User 1",
                Lastname = "Mustermann",
                Role = Roles.Admin
            };
            _user2 = new User("2")
            {
                Name = "User 2",
                Lastname = "Merkel",
                Role = Roles.Admin
            };

            _mudGame1 = new MudGame("1")
            {
                Name = "Game 1",
                Description = "Beschreibung Game 1",
                IsPublic = true,
                Owner = _user1
            };
            _user1.MudGames.Add(_mudGame1);

            _item1 = new Item("1")
            {
                Name = "Item 1",
                Description = "Beschreibung Item 1",
                Weight = 5,
                MudGame = _mudGame1
            };

            _context.Users.Add(_user2);
            _context.Items.Add(_item1);
            _context.MudGames.Add(_mudGame1);
            _context.SaveChanges();

            _createArgs = new ItemArgs()
            {
                Name = "Create Item",
                Description = "Create Beschreibung Item",
                Weight = 5,
                ImageKey = "123456789"
            };
            _updateArgs = new ItemArgs()
            {
                Name = "Update Item",
                Description = "Update Beschreibung Item",
                Weight = 10,
                ImageKey = "987654321"
            };
        }
    }
}
