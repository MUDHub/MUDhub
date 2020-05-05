using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Services;

namespace MUDhub.Core.Tests
{
    public class AreaManagerTests : IDisposable
    {

        private readonly AreaManager _areaManager;
        private readonly MudDbContext _context;
        private readonly MudGame _mudGame;
        private readonly Area _area;
        private readonly Room _room;
        private readonly Room _defaultRoom;
        private readonly RoomConnection _roomConnection;

        public AreaManagerTests()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_AreaManager")
                .Options;
            _context = new MudDbContext(options, useNotInUnitests: false);
            _areaManager = new AreaManager(_context);

            _mudGame = new MudGame("1")
            {
                Name = "Thors-Welt",
                DefaultRoom = _defaultRoom,
                Description = "Thors-Welt ist besser",
                IsPublic = true
            };
            _mudGame.Areas.Add(_area);

            _area = new Area("1")
            {
                Name = "Raknarok",
                GameId = "1",
                Description = "Heimatstadt von Thor",
                Game = _mudGame
            };

            _defaultRoom = new Room("1")
            {
                Name = "Schlafzimmer",
                Area = _area,
                GameId = "1",
                Description = "Warm und flauschig",
                X = 1,
                Y = 1
            };
            _defaultRoom.Connections.Add(_roomConnection);

            _room = new Room("2")
            {
                Name = "Esszimmer",
                GameId = "1",
                Area = _area,
                X = 2,
                Y = 1,
                Description = "Großer Holztisch"
            };
            _room.Connections.Add(_roomConnection);

            _roomConnection = new RoomConnection("1")
            {
                LockType = LockType.NoLock,
                Room1 = _defaultRoom,
                Room2 = _room,
                Description = "Der geheime Tunnel von Raknarok",
            };

            _area.Rooms.Add(_defaultRoom);
            _area.Rooms.Add(_room);
            _context.MudGames.Add(_mudGame);
            _context.Areas.Add(_area);
            _context.Rooms.Add(_defaultRoom);
            _context.Rooms.Add(_room);
            _context.RoomConnections.Add(_roomConnection);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.MudGames.Remove(_mudGame);
            _context.Areas.Remove(_area);
            _context.Rooms.Remove(_defaultRoom);
            _context.Rooms.Remove(_room);
            _context.RoomConnections.Remove(_roomConnection);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
