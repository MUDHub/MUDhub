using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Services
{
    public class AreaManager : IAreaManager
    {
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;

        public AreaManager(MudDbContext context, ILogger<AreaManager>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AreaResult> CreateAreaAsync(string userId, string mudId, AreaArgs args)
        {
            //Todo: Logger fehlt noch
            var user = await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }
            var mud = await _context.MudGames.FindAsync(mudId)
                .ConfigureAwait(false);
            if (mud is null)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "MUDGame is not found!"
                };
            }

            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == mud.Id);
            if (mudGameOwner is null)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            var area = new Area()
            {
                Name = args.Name,
                Description = args.Description,
                Game = mud,
            };
            _context.Areas.Add(area);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            return new AreaResult()
            {
                Area = area
            };
        }

        public Task<ConnectionResult> CreateConnectionAsync(string userId, Room room1, Room room2, LockArgs args)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomResult> CreateRoomAsync(string userId, string areaId, RoomArgs args)
        {
            //Todo: Logger fehlt noch und genauere Fehlermeldung
            var user = await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "Area is not found!"
                };
            }

            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == area.GameId);
            if (mudGameOwner is null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            var roomXY = await _context.Rooms.FirstOrDefaultAsync(r => r.X == args.X && r.Y == args.Y && r.Area == area)
                                             .ConfigureAwait(false);
            if (roomXY != null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "Da ist bereits ein Raum #Yolo"
                };
            }

            var room = new Room()
            {
                Name = args.Name,
                Description = args.Description,
                Area = area,
                ImageKey = args.ImageKey,
                X = args.X,
                Y = args.Y
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            return new RoomResult()
            {
                Room = room
            };
        }

        public async Task<AreaResult> RemoveAreaAsync(string userId, string areaId)
        {
            var user = await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }

            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "Area is not found!"
                };
            }

            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == area.GameId);
            if (mudGameOwner is null)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            var isDefault = area.Rooms.Any(r => r.Id == area.Game.DefaultRoomId);

            if (isDefault)
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "Area bestizt DefaultRoom",
                    Area = area
                };
            }
            foreach (Room room in area.Rooms)
            {
                await RemoveRoomAsync(userId, room.Id).ConfigureAwait(false);
                //Info: Es sollte keine Möglichkeit geben, dass die Ausführung fehlschlägt.
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            return new AreaResult()
            {
                Area = area
            };
        }

        public async Task<ConnectionResult> RemoveConnectionAsync(string userId, string connectionId)
        {
            var user = await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }






            throw new NotImplementedException();
        }

        public async Task<RoomResult> RemoveRoomAsync(string userId, string roomId)
        {
            var user = await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }

            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "Area is not found!"
                };
            }

            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == room.Area.GameId);
            if (mudGameOwner is null)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            var isDefault = room.Area.Game.DefaultRoomId == room.Id;
            if (isDefault)
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "Room ist DefaultRoom",
                    Room = room,
                    IsDefaultRoom = true
                };
            }

            foreach (RoomConnection roomConnection in room.Connections)
            {
                await RemoveConnectionAsync(userId, roomConnection.Id).ConfigureAwait(false);
                //Info: Es sollte keine Möglichkeit geben, dass die Ausführung fehlschlägt.
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            return new RoomResult()
            {
                Room = room
            };
        }

        public async Task<AreaResult> UpdateAreaAsync(string userId, string areaId)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomResult> UpdateRoomAsync(string userId, string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
