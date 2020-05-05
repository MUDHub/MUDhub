using System.Linq;
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
            var user = await GetUserById(userId).ConfigureAwait(false);
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

            if (await IsUserOwner(user, mud.Id).ConfigureAwait(false))
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

        public async Task<ConnectionResult> CreateConnectionAsync(string userId, string room1Id, string room2Id, RoomConnectionsArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }
            
            if (room1Id == room2Id)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "Die beiden Räume sind identisch"
                };
            }

            var room1 = await _context.Rooms.FindAsync(room1Id)
                .ConfigureAwait(false);
            if (room1 is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "Room1 is not found!"
                };
            }

            var room2 = await _context.Rooms.FindAsync(room2Id)
                .ConfigureAwait(false);
            if (room2 is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "Room2 is not found!"
                };
            }

            if (room1.GameId != room2.GameId)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "Räume sind nicht im gleichen MUD"
                };
            }

            if (await IsUserOwner(user, room1.GameId).ConfigureAwait(false))
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            var connection = new RoomConnection()
            {
                Room1 = room1,
                Room2 = room2,
                Description = args.Description,
                LockType = args.LockArgs.LockType,
                LockDescription = args.LockArgs.LockDescription,
                LockAssociatedId = args.LockArgs.LockAssociatedId
            };

            _context.RoomConnections.Add(connection);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            return new ConnectionResult()
            {
                RoomConnection = connection
            };
        }

        public async Task<RoomResult> CreateRoomAsync(string userId, string areaId, RoomArgs args)
        {
            //Todo: Logger fehlt noch und genauere Fehlermeldung
            var user = await GetUserById(userId).ConfigureAwait(false);
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

            if (await IsUserOwner(user, area.GameId).ConfigureAwait(false))
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
            var user = await GetUserById(userId).ConfigureAwait(false);
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

            if (await IsUserOwner(user, area.GameId).ConfigureAwait(false))
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
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }

            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "RoomConnection is not found!"
                };
            }

            if (await IsUserOwner(user, connection.Room1.GameId).ConfigureAwait(false))
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            _context.RoomConnections.Remove(connection);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            return new ConnectionResult()
            {
                RoomConnection = connection
            };

        }

        public async Task<RoomResult> RemoveRoomAsync(string userId, string roomId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
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

            if (await IsUserOwner(user, room.Area.GameId).ConfigureAwait(false))
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

        public async Task<AreaResult> UpdateAreaAsync(string userId, string areaId, UpdateAreaArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
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

            if (await IsUserOwner(user, area.GameId).ConfigureAwait(false))
            {
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            if (args.Name != null)
            {
                area.Name = args.Name;
            }

            if (args.Description != null)
            {
                area.Description = args.Description;
            }

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            return new AreaResult()
            {
                Area = area
            };
        }

        public async Task<ConnectionResult> UpdateConnectionAsync(string userId, string connectionId, UpdateRoomConnectionsArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User is not found!"
                };
            }
            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "RoomConnection is not found!"
                };
            }

            if (await IsUserOwner(user, connection.Room1.GameId).ConfigureAwait(false))
            {
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            if (args.Description != null)
            {
                connection.Description = args.Description;
            }

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            return new ConnectionResult()
            {
                RoomConnection = connection
            };
        }

        public async Task<RoomResult> UpdateRoomAsync(string userId, string roomId, UpdateRoomArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
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
                    Errormessage = "User is not found!"
                };
            }

            if (await IsUserOwner(user, room.GameId).ConfigureAwait(false))
            {
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = "User ist nicht berechtigt"
                };
            }

            if (args.Name != null)
            {
                room.Name = args.Name;
            }

            if (args.Description != null)
            {
                room.Description = args.Description;
            }

            if (args.ImageKey != null)
            {
                room.ImageKey = args.ImageKey;
            }

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            return new RoomResult()
            {
                Room = room
            };
        }




        private async Task<bool> IsUserOwner(User user, string gameId)
        {
            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == gameId);
            return !(mudGameOwner is null);
        }

        private async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
        }
    }
}
