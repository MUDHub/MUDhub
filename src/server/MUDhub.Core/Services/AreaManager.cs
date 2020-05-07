using System.Collections.Generic;
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

        /// <summary>
        /// A new area is created in the MudGame.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mudId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<AreaResult> CreateAreaAsync(string userId, string mudId, AreaArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }
            var mud = await _context.MudGames.FindAsync(mudId)
                .ConfigureAwait(false);
            if (mud is null)
            {
                _logger?.LogWarning($"No MUDGame found with the MudGameId: {mudId}");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"No MUDGame found with the MudGameId: {mudId}"
                };
            }

            if (!IsUserOwner(user, mud.Id))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {mud.Name}");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {mud.Name}"
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
            _logger?.LogInformation($"The area: {area.Name} was created in MudGame: {mud.Name}");
            return new AreaResult()
            {
                Area = area
            };
        }


        /// <summary>
        /// A new connection is created between two rooms.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room1Id"></param>
        /// <param name="room2Id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<ConnectionResult> CreateConnectionAsync(string userId, string room1Id, string room2Id, RoomConnectionsArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }

            if (room1Id == room2Id)
            {
                _logger?.LogWarning($"Rooms {room1Id} and {room2Id} are identical. A connection is not possible.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"Rooms {room1Id} and {room2Id} are identical. A connection is not possible."
                };
            }

            var room1 = await _context.Rooms.FindAsync(room1Id)
                .ConfigureAwait(false);
            if (room1 is null)
            {
                _logger?.LogWarning($"No Room found with the RoomId: {room1Id}");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No Room found with the RoomId: {room1Id}"
                };
            }

            var room2 = await _context.Rooms.FindAsync(room2Id)
                .ConfigureAwait(false);
            if (room2 is null)
            {
                _logger?.LogWarning($"No Room found with the RoomId: {room2Id}");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No Room found with the RoomId: {room2Id}"
                };
            }
            if (room1.GameId != room2.GameId)
            {
                _logger?.LogWarning($"The rooms {room1.Name} and {room2.Name} are not in the same MudGame.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"The rooms {room1.Name} and {room2.Name} are not in the same MudGame."
                };
            }

            if (!IsUserOwner(user, room1.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {room1.GameId}");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {room1.GameId}"
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

            //_logger?.LogInformation($"A connection: {connection.Id} was created between room: {room1.Name} and room: {room2.Name}");
            return new ConnectionResult()
            {
                RoomConnection = connection
            };
        }

        /// <summary>
        /// A new room is created in the MudGame.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="areaId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RoomResult> CreateRoomAsync(string userId, string areaId, RoomArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                _logger?.LogWarning($"No area with the AreaId: {areaId} was found.");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"No area with the AreaId: {areaId} was found."
                };
            }

            if (!IsUserOwner(user, area.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {area.Game.Name}");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {area.Game.Name}"
                };
            }

            var roomXy = await _context.Rooms.FirstOrDefaultAsync(r => r.X == args.X && r.Y == args.Y && r.Area == area)
                                             .ConfigureAwait(false);
            if (roomXy != null)
            {
                _logger?.LogWarning($"At position X: {args.X} and Y: {args.Y} there is already a room");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"At position X: {args.X} and Y: {args.Y} there is already a room"
                };
            }

            if (args.IsDefaultRoom)
            {
                foreach (var otherRoom in area.Rooms)
                {
                    otherRoom.IsDefaultRoom = false;
                }
            }

            var room = new Room()
            {
                Name = args.Name,
                Description = args.Description,
                Area = area,
                ImageKey = args.ImageKey,
                X = args.X,
                Y = args.Y,
                IsDefaultRoom = args.IsDefaultRoom
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"A room: {room.Id} was created in area: {area.Name}");
            return new RoomResult()
            {
                Room = room
            };
        }


        /// <summary>
        /// An area is being removed from the MudGame.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public async Task<AreaResult> RemoveAreaAsync(string userId, string areaId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }

            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                _logger?.LogWarning($"No area with the AreaId: {areaId} was found.");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"No area with the AreaId: {areaId} was found."
                };
            }

            if (!IsUserOwner(user, area.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {area.Game.Name}");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {area.Game.Name}"
                };
            }

            var isDefault = area.Rooms.Any(r => r.IsDefaultRoom);
            if (isDefault)
            {
                _logger?.LogWarning($"Area: {area.Name} could not be deleted because it has a default room");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"Area: {area.Name} could not be deleted because it has a default room",
                    Area = area
                };
            }

            List<Room> roomList = area.Rooms.ToList();
            foreach (Room room in roomList)
            {
                await RemoveRoomAsync(userId, room.Id).ConfigureAwait(false);
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The area: {area.Name} has been removed from the MudGame: {area.Game.Name}");
            return new AreaResult()
            {
                Area = area
            };
        }

        /// <summary>
        /// A connection is removed from the MudGame.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public async Task<ConnectionResult> RemoveConnectionAsync(string userId, string connectionId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }

            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                _logger?.LogWarning($"No room connection with the connectionId: {connectionId} was found.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No room connection with the connectionId: {connectionId} was found."
                };
            }

            if (!IsUserOwner(user, connection.Room1.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {connection.Room1.GameId}");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {connection.Room1.GameId}"
                };
            }

            _context.RoomConnections.Remove(connection);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            //_logger?.LogInformation($"The room connection: {connection.Id} has been removed from the MudGame: {connection.Room1.GameId}");
            return new ConnectionResult()
            {
                RoomConnection = connection
            };
        }

        /// <summary>
        /// A room is removed from the MudGame.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<RoomResult> RemoveRoomAsync(string userId, string roomId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }

            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                _logger?.LogWarning($"No room with the RoomId: {roomId} was found.");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"No room with the RoomId: {roomId} was found."
                };
            }

            if (!IsUserOwner(user, room.Area.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {room.GameId}");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {room.GameId}"
                };
            }

            if (room.IsDefaultRoom)
            {
                _logger?.LogWarning($"Room: {room.Name} could not be deleted because it is a default room");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"Room: {room.Name} could not be deleted because it is a default room",
                    Room = room,
                    IsDefaultRoom = true
                };
            }

            List<RoomConnection> roomConnectionsList = room.Connections.ToList();
            //foreach (RoomConnection roomConnection in roomConnectionsList)
            //{
            //    await RemoveConnectionAsync(userId, roomConnection.Id).ConfigureAwait(false);
            //}

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The room: {room.Name} has been removed from the MudGame: {room.GameId}");
            return new RoomResult()
            {
                Room = room
            };
        }

        /// <summary>
        /// An area is updated.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="areaId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<AreaResult> UpdateAreaAsync(string userId, string areaId, UpdateAreaArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }

            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                _logger?.LogWarning($"No area with the AreaId: {areaId} was found.");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"No area with the AreaId: {areaId} was found."
                };
            }

            if (!IsUserOwner(user, area.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {area.Game.Name}");
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {area.Game.Name}"
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
            _logger?.LogInformation($"The area: {area.Name} was updated");
            return new AreaResult()
            {
                Area = area
            };
        }

        /// <summary>
        /// An connection is updated.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<ConnectionResult> UpdateConnectionAsync(string userId, string connectionId, UpdateRoomConnectionsArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }
            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                _logger?.LogWarning($"No room connection with the connectionId: {connectionId} was found.");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"No room connection with the connectionId: {connectionId} was found."
                };
            }

            if (!IsUserOwner(user, connection.Room1.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {connection.Room1.GameId}");
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {connection.Room1.GameId}"
                };
            }

            if (args.Description != null)
            {
                connection.Description = args.Description;
            }

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            //Todo: update connectionid
            //_logger?.LogInformation($"The room connection: {connection.room} was updated");
            return new ConnectionResult()
            {
                RoomConnection = connection
            };
        }

        /// <summary>
        /// An room is updated.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RoomResult> UpdateRoomAsync(string userId, string roomId, UpdateRoomArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"No user with the UserId: {userId} was found.");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"No user with the UserId: {userId} was found."
                };
            }

            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                _logger?.LogWarning($"No room with the RoomId: {roomId} was found.");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"No room with the RoomId: {roomId} was found."
                };
            }

            if (!IsUserOwner(user, room.GameId))
            {
                _logger?.LogWarning($"The user: {user.Lastname} is not the owner of the MudGame: {room.GameId}");
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = $"The user: {user.Lastname} is not the owner of the MudGame: {room.GameId}"
                };
            }
            if (args.IsDefaultRoom)
            {
                foreach (var otherRoom in room.Area.Rooms)
                {
                    otherRoom.IsDefaultRoom = false;
                }
            }

            room.IsDefaultRoom = args.IsDefaultRoom;

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
            _logger?.LogInformation($"The room: {room.Name} was updated");
            return new RoomResult()
            {
                Room = room
            };
        }


        /// <summary>
        /// Is the user really the owner of the MudGame?
        /// </summary>
        /// <param name="user"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        private bool IsUserOwner(User user, string gameId)
        {
            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == gameId);
            return !(mudGameOwner is null);
        }


        /// <summary>
        /// The UserId is used to determine the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
        }
    }
}
