using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Users;
using MUDhub.Core.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class InventoryServiceTests : IDisposable
    {
        private InventoryService _inventoryService;
        private MudDbContext _context;

        private User _user1, _user2;
        private Inventory _inventory1, _inventory2;
        private Item _item1, _item2;
        private ItemInstance _itemInstance1, _itemInstance2;

        public InventoryServiceTests()
        {
            InitializeTestDb();
        }

        public void Dispose()
        {
            _context.Users.Remove(_user1);
            //_context.MudGames.Remove(_mudGame1);
            _context.SaveChanges();
            _context.Dispose();
        }

        //*********************************************************//

        [Fact]
        public async Task CreateItemInstance_ReturnFalse_UserNull()
        {
            var result = await _inventoryService.CreateItemInstance("99", "1", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemInstance_ReturnFalse_NoAuthorization()
        {
            var result = await _inventoryService.CreateItemInstance("2", "1", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemInstance_ReturnFalse_InventoryNull()
        {
            var result = await _inventoryService.CreateItemInstance("1", "99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemInstance_ReturnFalse_ItemNull()
        {
            var result = await _inventoryService.CreateItemInstance("1", "1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemInstance_ReturnFalse_FullCapacity()
        {
            var result = await _inventoryService.CreateItemInstance("1", "2", "2");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateItemInstance_ReturnTrue()
        {
            var result = await _inventoryService.CreateItemInstance("1", "1", "1");
            Assert.True(result.Success);
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveItemInstance_ReturnFalse_UserNull()
        {
            var result = await _inventoryService.RemoveItemInstance("99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveItemInstance_ReturnFalse_ItemInstanceNull()
        {
            var result = await _inventoryService.RemoveItemInstance("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveItemInstance_ReturnTrue()
        {
            var result = await _inventoryService.RemoveItemInstance("1", "1");
            Assert.True(result.Success);
        }

        //*********************************************************//

        [Fact]
        public async Task TransferItem_ReturnFalse_ItemInstanceNull()
        {
            var result = await _inventoryService.TransferItem("99", "1", "2");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task TransferItem_ReturnFalse_TargetInventoryNull()
        {
            var result = await _inventoryService.TransferItem("1", "99", "2");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task TransferItem_ReturnFalse_SourceInventoryNull()
        {
            var result = await _inventoryService.TransferItem("1", "1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task TransferItem_ReturnFalse_NoItemInstanceInSourceInventory()
        {
            var result = await _inventoryService.TransferItem("1", "1", "2");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task TransferItem_ReturnFalse_TargetInventoryToLittle()
        {
            var result = await _inventoryService.TransferItem("1", "2", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task TransferItem_ReturnTrue()
        {
            var result = await _inventoryService.TransferItem("2", "1", "2");
            Assert.True(result.Success);
        }

        //*********************************************************//

        private void InitializeTestDb()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_ItemManager")
                .Options;
            _context = new MudDbContext(options, useNotInUnitests: false);
            _inventoryService = new InventoryService(_context);

            _user1 = new User("1")
            {
                Role = Roles.Master,
                Name = "User 1",
                Lastname = "Mustermann",
                Email = "user1@mustermann.de"
            };
            _user2 = new User("2")
            {
                Role = Roles.Player,
                Name = "User 2",
                Lastname = "Musterfrau",
                Email = "user1@musterfrau.de"
            };

            _item1 = new Item("1")
            {
                Name = "Item 1",
                Description = "Beschreibung Item 1",
                Weight = 15
            };
            _item2 = new Item("2")
            {
                Name = "Item 2",
                Description = "Beschreibung Item 2",
                Weight = 20
            };

            _inventory1 = new Inventory("1")
            {
                Capacity = int.MaxValue
            }
            ;
            _inventory2 = new Inventory("2")
            {
                UsedCapacity = 190
            };

            _itemInstance1 = new ItemInstance("1")
            {
                Inventory = _inventory1,
                Item = _item1
            };
            _itemInstance2 = new ItemInstance("2")
            {
                Inventory = _inventory2,
                Item = _item2
            };

            _inventory1.ItemInstances.Add(_itemInstance1);
            _inventory2.ItemInstances.Add(_itemInstance2);

            _context.Users.Add(_user2);
            _context.Users.Add(_user1);
            _context.Inventories.Add(_inventory1);
            _context.Inventories.Add(_inventory2);
            _context.Items.Add(_item2);
            //_context.Users.Add(_user1);
            //_context.Users.Add(_user2);
            //_context.Users.Add(_user1);
            //_context.MudGames.Add(_mudGame1);
            _context.SaveChanges();
        }
    }
}
