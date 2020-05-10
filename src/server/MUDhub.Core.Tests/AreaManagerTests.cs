using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts.Models.Areas;
using MUDhub.Core.Abstracts.Models.Connections;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Models.Users;
using MUDhub.Core.Services;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class AreaManagerTests : IDisposable
    {

        private AreaManager _areaManager;
        private MudDbContext _context;
        private User _user1, _user2;
        private MudGame _mudGame1, _mudGame2;
        private Area _area1, _area2, _area3;
        private Room _room1Default, _room2, _room3, _room4, _room5, _room6Default;
        private RoomConnection _connection1;

        private RoomConnectionsArgs _roomConnectionsArgs;
        private AreaArgs _areaArgs;
        private RoomArgs _roomArgs;
        private UpdateAreaArgs _updateAreaArgs;
        private UpdateRoomConnectionsArgs _updateRoomConnectionsArgs;
        private UpdateRoomArgs _updateRoomArgs;


        public AreaManagerTests()
        {
            InitializeTestDb();
        }
        public void Dispose()
        {
            _context.MudGames.Remove(_mudGame1);
            _context.MudGames.Remove(_mudGame2);
            _context.SaveChanges();
            _context.Dispose();
        }

        //*********************************************************//

        [Fact]
        public async Task CreateAreaAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.CreateAreaAsync("99", "1", _areaArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateAreaAsync_ReturnFalse_MudNull()
        {
            var result = await _areaManager.CreateAreaAsync("1", "99", _areaArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateAreaAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.CreateAreaAsync("2", "1", _areaArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateAreaAsync_ReturnTrue()
        {
            var result = await _areaManager.CreateAreaAsync("1", "1", _areaArgs);
            Assert.True(result.Success);

            var newArea = _context.Areas.FirstOrDefault(a => a.Name.Equals("New Area"));
            Assert.True(newArea.Description.Equals("Beschreibung New Area"));
        }

        //*********************************************************//

        [Fact]
        public async Task CreateConnectionAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.CreateConnectionAsync("99", "2", "3", _roomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateConnectionAsync_ReturnFalse_RoomIdsAreEqual()
        {
            var result = await _areaManager.CreateConnectionAsync("1", "2", "2", _roomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateConnectionAsync_ReturnFalse_Room1Null()
        {
            var result = await _areaManager.CreateConnectionAsync("1", "99", "3", _roomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateConnectionAsync_ReturnFalse_Room2Null()
        {
            var result = await _areaManager.CreateConnectionAsync("1", "2", "99", _roomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateConnectionAsync_ReturnFalse_RoomsNotInTheSameMud()
        {
            var result = await _areaManager.CreateConnectionAsync("1", "2", "6", _roomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateConnectionAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.CreateConnectionAsync("2", "2", "3", _roomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateConnectionAsync_ReturnTrue()
        {
            var result = await _areaManager.CreateConnectionAsync("1", "2", "3", _roomConnectionsArgs);
            Assert.True(result.Success);

            var newConnection = _context.RoomConnections.FirstOrDefault(
                a => a.Description.Equals("Beschreibung New Connection"));
            Assert.True(newConnection.LockDescription.Equals("Beschreibung New Lock"));
        }

        //*********************************************************//

        [Fact]
        public async Task CreateRoomAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.CreateRoomAsync("99", "1", _roomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateRoomAsync_ReturnFalse_AreaNull()
        {
            var result = await _areaManager.CreateRoomAsync("1", "4", _roomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateRoomAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.CreateRoomAsync("2", "1", _roomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateRoomAsync_ReturnFalse_XAndYAreOccupied()
        {
            _roomArgs.X = 1;
            _roomArgs.Y = 1;
            var result = await _areaManager.CreateRoomAsync("1", "1", _roomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateRoomAsync_ReturnTrue_ChangeDefaultRoom()
        {
            _roomArgs.IsDefaultRoom = true;
            var result = await _areaManager.CreateRoomAsync("1", "1", _roomArgs);
            Assert.True(result.Success);

            var oldDefaultRoom = _context.Rooms.FirstOrDefault(
                a => a.Name.Equals("Room 1"));
            Assert.False(oldDefaultRoom.IsDefaultRoom);

            var oldDefaultRoom2 = _context.Rooms.FirstOrDefault(
                a => a.Name.Equals("Room 6"));
            Assert.True(oldDefaultRoom2.IsDefaultRoom);
        }
        [Fact]
        public async Task CreateRoomAsync_ReturnTrue()
        {
            var result = await _areaManager.CreateRoomAsync("1", "1", _roomArgs);
            Assert.True(result.Success);

            var newRoom = _context.Rooms.FirstOrDefault(
                a => a.Name.Equals("New Room"));
            Assert.True(newRoom.Description.Equals("Beschreibung New Room"));
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveAreaAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.RemoveAreaAsync("99", "2");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveAreaAsync_ReturnFalse_AreaNull()
        {
            var result = await _areaManager.RemoveAreaAsync("1", "4");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveAreaAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.RemoveAreaAsync("1", "3");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveAreaAsync_ReturnFalse_IncludesDefaultRoom()
        {
            var result = await _areaManager.RemoveAreaAsync("1", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveAreaAsync_ReturnTrue()
        {
            var result = await _areaManager.RemoveAreaAsync("1", "2");
            Assert.True(result.Success);

            var oldArea = _context.Areas.FirstOrDefault(
                a => a.Name.Equals("Area 2"));
            Assert.True(oldArea is null);
            var oldRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("4"));
            Assert.True(oldRoom is null);
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveConnectionAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.RemoveConnectionAsync("99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveConnectionAsync_ReturnFalse_ConnectionNull()
        {
            var result = await _areaManager.RemoveConnectionAsync("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveConnectionAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.RemoveConnectionAsync("2", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveConnectionAsync_ReturnTrue()
        {
            var result = await _areaManager.RemoveConnectionAsync("1", "1");
            Assert.True(result.Success);

            var oldConnection = _context.RoomConnections.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(oldConnection is null);
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveRoomAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.RemoveRoomAsync("99", "3");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveRoomAsync_ReturnFalse_RoomNull()
        {
            var result = await _areaManager.RemoveRoomAsync("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveRoomAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.RemoveRoomAsync("2", "4");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveRoomAsync_ReturnFalse_IsDefaultRoom()
        {
            var result = await _areaManager.RemoveRoomAsync("1", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveRoomAsync_ReturnTrue()
        {
            var result = await _areaManager.RemoveRoomAsync("1", "2");
            Assert.True(result.Success);

            var oldConnection = _context.RoomConnections.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(oldConnection is null);
            var oldRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("2"));
            Assert.True(oldRoom is null);
        }

        //*********************************************************//

        [Fact]
        public async Task UpdateAreaAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.UpdateAreaAsync("99", "2", _updateAreaArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateAreaAsync_ReturnFalse_AreaNull()
        {
            var result = await _areaManager.UpdateAreaAsync("1", "99", _updateAreaArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateAreaAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.UpdateAreaAsync("1", "3", _updateAreaArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateAreaAsync_ReturnTrue()
        {
            var result = await _areaManager.UpdateAreaAsync("1", "2", _updateAreaArgs);
            Assert.True(result.Success);

            var newArea = _context.Areas.FirstOrDefault(
                a => a.Id.Equals("2"));
            Assert.True(newArea.Name.Equals("New Area"));
            Assert.True(newArea.Description.Equals("Beschreibung New Area"));
        }

        //*********************************************************//

        [Fact]
        public async Task UpdateConnectionAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.UpdateConnectionAsync("99", "1", _updateRoomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateConnectionAsync_ReturnFalse_ConnectionNull()
        {
            var result = await _areaManager.UpdateConnectionAsync("1", "99", _updateRoomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateConnectionAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.UpdateConnectionAsync("2", "1", _updateRoomConnectionsArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateConnectionAsync_ReturnTrue()
        {
            var result = await _areaManager.UpdateConnectionAsync("1", "1", _updateRoomConnectionsArgs);
            Assert.True(result.Success);

            var newConnection = _context.RoomConnections.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(newConnection.Description.Equals("Beschreibung New Connection"));
        }

        //*********************************************************//

        [Fact]
        public async Task UpdateRoomAsync_ReturnFalse_UserNull()
        {
            var result = await _areaManager.UpdateRoomAsync("99", "1", _updateRoomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateRoomAsync_ReturnFalse_RoomNull()
        {
            var result = await _areaManager.UpdateRoomAsync("1", "99", _updateRoomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateRoomAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _areaManager.UpdateRoomAsync("2", "1", _updateRoomArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateRoomAsync_ReturnTrue_ChangeDefaultRoom()
        {
            var beforeRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("2"));
            var beforeDefaultRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(!beforeRoom.IsDefaultRoom);
            Assert.True(beforeDefaultRoom.IsDefaultRoom);

            _updateRoomArgs.IsDefaultRoom = true;
            var result = await _areaManager.UpdateRoomAsync("1", "2", _updateRoomArgs);
            Assert.True(result.Success);

            var afterRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("2"));
            var afterDefaultRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(afterRoom.IsDefaultRoom);
            Assert.True(!afterDefaultRoom.IsDefaultRoom);
        }
        [Fact]
        public async Task UpdateRoomAsync_ReturnTrue()
        {
            var result = await _areaManager.UpdateRoomAsync("1", "1", _updateRoomArgs);
            Assert.True(result.Success);

            var newRoom = _context.Rooms.FirstOrDefault(
                a => a.Id.Equals("1"));
            Assert.True(newRoom.Description.Equals("Beschreibung New Room"));
            Assert.True(newRoom.Name.Equals("New Room"));
            Assert.True(newRoom.ImageKey.Equals("New Room ImageKey"));
        }


        private void InitializeTestDb()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_AreaManager")
                .Options;
            _context = new MudDbContext(options, useNotInUnitests: false);
            _areaManager = new AreaManager(_context);

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

            _mudGame2 = new MudGame("2")
            {
                Name = "Game 2",
                Description = "Beschreibung Game 2",
                IsPublic = true,
                Owner = _user1
            };
            _user2.MudGames.Add(_mudGame2);


            _area1 = new Area("1")
            {
                Name = "Area 1",
                GameId = "1",
                Description = "Beschreibung Area 1",
                Game = _mudGame1
            };
            _mudGame1.Areas.Add(_area1);

            _area2 = new Area("2")
            {
                Name = "Area 2",
                GameId = "1",
                Description = "Beschreibung Area 2",
                Game = _mudGame1
            };
            _mudGame1.Areas.Add(_area2);

            _area3 = new Area("3")
            {
                Name = "Area 3",
                GameId = "2",
                Description = "Beschreibung Area 3",
                Game = _mudGame2
            };
            _mudGame2.Areas.Add(_area3);

            _room1Default = new Room("1")
            {
                Name = "Room 1",
                Area = _area1,
                GameId = "1",
                Description = "Beschreibung Room 1",
                X = 1,
                Y = 1,
                IsDefaultRoom = true
            };
            _area1.Rooms.Add(_room1Default);

            _room2 = new Room("2")
            {
                Name = "Room 2",
                Area = _area1,
                GameId = "1",
                Description = "Beschreibung Room 2",
                X = 2,
                Y = 1
            };
            _area1.Rooms.Add(_room2);

            _room3 = new Room("3")
            {
                Name = "Room 3",
                Area = _area1,
                GameId = "1",
                Description = "Beschreibung Room 3",
                X = 3,
                Y = 1
            };
            _area1.Rooms.Add(_room3);

            _room4 = new Room("4")
            {
                Name = "Room 4",
                Area = _area2,
                GameId = "1",
                Description = "Beschreibung Room 4",
                X = 1,
                Y = 1
            };
            _area2.Rooms.Add(_room4);

            _room5 = new Room("5")
            {
                Name = "Room 5",
                Area = _area2,
                GameId = "1",
                Description = "Beschreibung Room 5",
                X = 2,
                Y = 1
            };
            _area2.Rooms.Add(_room5);

            _room6Default = new Room("6")
            {
                Name = "Room 6",
                Area = _area3,
                GameId = "2",
                Description = "Beschreibung Room 6",
                X = 1,
                Y = 1,
                IsDefaultRoom = true
            };
            _area3.Rooms.Add(_room6Default);

            _connection1 = new RoomConnection("1")
            {
                LockType = LockType.NoLock,
                Room1 = _room1Default,
                Room2 = _room2,
                Description = "Connection von Room1 zu Room2",
            };

            _room1Default.Connections1.Add(_connection1);
            _room2.Connections1.Add(_connection1);

            _context.Users.Add(_user2);
            _context.MudGames.Add(_mudGame2);
            _context.MudGames.Add(_mudGame1);
            _context.SaveChanges();

            _roomConnectionsArgs = new RoomConnectionsArgs()
            {
                Description = "Beschreibung New Connection",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock,
                    LockDescription = "Beschreibung New Lock"
                }
            };
            _areaArgs = new AreaArgs()
            {
                Name = "New Area",
                Description = "Beschreibung New Area"
            };
            _roomArgs = new RoomArgs()
            {
                Name = "New Room",
                Description = "Beschreibung New Room",
                ImageKey = "New Room Image Key"
            };
            _updateAreaArgs = new UpdateAreaArgs()
            {
                Name = "New Area",
                Description = "Beschreibung New Area"
            };
            _updateRoomConnectionsArgs = new UpdateRoomConnectionsArgs()
            {
                Description = "Beschreibung New Connection"
            };
            _updateRoomArgs = new UpdateRoomArgs()
            {
                Name = "New Room",
                Description = "Beschreibung New Room",
                ImageKey = "New Room ImageKey"
            };
        }
    }
}
