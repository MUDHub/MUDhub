using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Areas;
using MUDhub.Core.Abstracts.Models.Connections;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }
            var mud = await _context.MudGames.FindAsync(mudId)
                .ConfigureAwait(false);
            if (mud is null)
            {
                var message = $"No MUDGame found with the MudGameId: '{mudId}'";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein MudGame mit der MudGame Id: '{mudId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, mud.Id))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{mud.Name}'";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{mud.Name}'."
                };
            }

            var area = new Area()
            {
                Name = args.Name,
                Description = args.Description,
                Game = mud
            };
            _context.Areas.Add(area);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The area: '{area.Name}' with id '{area.Id}' was created in MudGame: '{mud.Name}' from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            if (room1Id == room2Id)
            {
                var message = $"Rooms '{room1Id}' and '{room2Id}' are identical. A connection is not possible.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Die beiden Räume haben die gleiche Id: '{room1Id}'. Eine Verbindung ist nicht möglich"
                };
            }

            var room1 = await _context.Rooms.FindAsync(room1Id)
                .ConfigureAwait(false);
            if (room1 is null)
            {
                var message = $"No Room found with the RoomId: '{room1Id}'";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Raum mit der Raum-Id: '{room1Id}' wurde gefunden."
                };
            }

            var room2 = await _context.Rooms.FindAsync(room2Id)
                .ConfigureAwait(false);
            if (room2 is null)
            {
                var message = $"No Room found with the RoomId: '{room2Id}'";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Raum mit der Raum Id: '{room1Id}' wurde gefunden."
                };
            }
            if (room1.GameId != room2.GameId)
            {
                var message = $"The rooms '{room1.Name}' and '{room2.Name}' are not in the same MudGame.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Raum: '{room1.Name}' und der Raum: '{room2.Name}' sind nicht im selben MudGame."
                };
            }

            if (!IsUserOwner(user, room1.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{room1.GameId}'";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{room1.GameId}'."
                };
            }

            var connection = new RoomConnection()
            {
                Room1 = room1,
                Room2 = room2,
                Description = args.Description,
                LockType = args.LockArgs.LockType,
                LockDescription = args.LockArgs.LockDescription,
                LockAssociatedId = args.LockArgs.LockAssociatedId,
                GameId = room1.GameId
            };

            _context.RoomConnections.Add(connection);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            _logger?.LogInformation($"A connection: {connection.Id} was created between room: {room1.Name} and room: {room2.Name} from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                var message = $"No area with the AreaId: '{areaId}' was found.";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Keine Area mit der Area Id: '{areaId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, area.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{area.Game.Name}'";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{area.Game.Name}'."
                };
            }

            var roomXy = await _context.Rooms.FirstOrDefaultAsync(r => r.X == args.X && r.Y == args.Y && r.Area == area)
                                             .ConfigureAwait(false);
            if (roomXy != null)
            {
                var message = $"At position X: '{args.X}' and Y: '{args.Y}' there is already a room";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"An der Position X: '{args.X}' und Y: '{args.Y}' befindet sich bereits ein Raum."
                };
            }

            if (args.IsDefaultRoom)
            {
                foreach (var otherRoom in area.Game.Areas.SelectMany(a => a.Rooms))
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
                IsDefaultRoom = args.IsDefaultRoom,
                Game = area.Game,
                Inventory = new Inventory()
                {
                    Capacity = int.MaxValue
                }
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"A room: '{room.Id}' was created in area: '{area.Name}' from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                var message = $"No area with the AreaId: '{areaId}' was found.";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Keine Area mit der Area Id: '{areaId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, area.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{area.Game.Name}'";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{area.Game.Name}'."
                };
            }

            var isDefault = area.Rooms.Any(r => r.IsDefaultRoom);
            if (isDefault)
            {
                var message = $"Area: {area.Name} could not be deleted because it has a default room";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    Area = area,
                    DisplayMessage = $"Das Gebiet: '{area.Name}' konnte nicht gelöscht werde, da es den Eintrittsraum enthält."
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
            _logger?.LogInformation($"The area: '{area.Name}' has been removed from the MudGame: '{area.Game.Name}' from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                var message = $"No room connection with the connectionId: '{connectionId}' was found.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Raum-Verbindung mit der Verbindungs-Id: '{connectionId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, connection.Room1.GameId))
            {
                var message = $"The user: {user.Lastname} is not the owner of the MudGame: {connection.Room1.GameId}";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{connection.Room1.Game.Name}'."
                };
            }

            _context.RoomConnections.Remove(connection);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The room connection: '{connection.Id}' has been removed from the MudGame: '{connection.Room1.GameId}' from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                var message = $"No room with the RoomId: '{roomId}' was found.";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Raum mit der Raum Id: '{roomId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, room.Area.GameId))
            {
                var message = $"The user: {user.Lastname} is not the owner of the MudGame: {room.GameId}";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{room.Game.Name}'."
                };
            }

            if (room.IsDefaultRoom)
            {
                var message = $"Room: '{room.Name}' could not be deleted because it is a default room";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    Room = room,
                    IsDefaultRoom = true,
                    DisplayMessage = $"Der Raum: '{room.Name}' konnte nicht gelöscht werden, da er ein Eintrittsraum ist."
                };
            }

            //List<RoomConnection> roomConnectionsList = room.Connections1.ToList();
            //foreach (RoomConnection roomConnection in roomConnectionsList)
            //{
            //    await RemoveConnectionAsync(userId, roomConnection.Id).ConfigureAwait(false);
            //}

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The room: '{room.Name}' with the id: '{room.Id}' has been removed from the MudGame: '{room.Game.Name}' with the id '{room.GameId}' from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                var message = $"No area with the AreaId: '{areaId}' was found.";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Bereih mit der Bereich Id: '{areaId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, area.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{area.Game.Name}'";
                _logger?.LogWarning(message);
                return new AreaResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{area.Game.Name}'."
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
            _logger?.LogInformation($"The area: '{area.Name}' was updated from '{user.Email}' {Environment.NewLine}" +
                $"- Name: {args.Name} {Environment.NewLine}" +
                $"- Description: {args.Description}");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }
            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                var message = $"No room connection with the connectionId: '{connectionId}' was found.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Keine Raum-Verbindung mit der Verbindungs Id: '{connectionId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, connection.Room1.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{connection.Room1.GameId}'.";
                _logger?.LogWarning(message);
                return new ConnectionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{connection.Room1.Game.Name}'."
                };
            }
            connection.Description = args.Description;
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The room connection: '{connection.Id}' was updated from '{user.Email}'");
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
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                var message = $"No room with the RoomId: '{roomId}' was found.";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Raum mit der Raum Id: '{roomId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, room.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{room.GameId}'";
                _logger?.LogWarning(message);
                return new RoomResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{room.Game.Name}'."
                };
            }
            if (args.IsDefaultRoom)
            {
                foreach (var otherRoom in room.Area.Game.Areas.SelectMany(r => r.Rooms))
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
            _logger?.LogInformation($"The room: '{room.Name}' was updated from '{user.Email}': {Environment.NewLine}" +
                $"- Name: {args.Name ?? "<no modification>"} {Environment.NewLine}" +
                $"- Description: {args.Description ?? "<no modification>"} {Environment.NewLine}" + 
                $"- ImageKey: {args.ImageKey ?? "<no modification>"}");
            return new RoomResult()
            {
                Room = room
            };
        }

        /// <summary>
        /// A room interaction is added to the room.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RoomInteractionResult> CreateRoomInteractionAsync(string userId, string roomId, RoomInteractionArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new RoomInteractionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }

            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                var message = $"No Room found with the RoomId: '{roomId}'";
                _logger?.LogWarning(message);
                return new RoomInteractionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Raum mit der Raum Id: '{roomId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, room.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{room.Game.Name}'";
                _logger?.LogWarning(message);
                return new RoomInteractionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{room.Game.Name}'."
                };
            }

            var roomInteraction = new RoomInteraction()
            {
                Description = args.Description,
                ExecutionMessage = args.ExecutionMessage,
                RelatedId = args.RelatedId,
                Type = args.Type
            };

            _context.RoomInteractions.Add(roomInteraction);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"A room interaction: '{roomInteraction.Description}' with Id: '{roomInteraction.Id}' was created in MudGame: '{room.Game.Name}' from '{user.Email}'");
            return new RoomInteractionResult()
            {
                RoomInteraction = roomInteraction
            };
        }

        /// <summary>
        /// A room interaction is removed from the room.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomInteractionId"></param>
        /// <returns></returns>
        public async Task<RoomInteractionResult> RemoveRoomInteractionAsync(string userId, string roomInteractionId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new RoomInteractionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Benutzer mit der Benutzer Id: '{userId}' wurde gefunden."
                };
            }
            var roomInteraction = await _context.RoomInteractions.FindAsync(roomInteractionId)
                .ConfigureAwait(false);
            if (roomInteraction is null)
            {
                var message = $"No room interaction found with the RoomInteractionId: '{roomInteractionId}'";
                _logger?.LogWarning(message);
                return new RoomInteractionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Keine Raum-Interaktion mit der Interaktions-Id: '{roomInteractionId}' wurde gefunden."
                };
            }

            if (!IsUserOwner(user, roomInteraction.GameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{roomInteraction.GameId}'";
                _logger?.LogWarning(message);
                return new RoomInteractionResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der Benutzer: '{user.Lastname}' ist nicht der Besitzer des MudGames: '{roomInteraction.Game.Name}'."
                };
            }

            _context.RoomInteractions.Remove(roomInteraction);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The room interaction: '{roomInteraction.Description}' with Id: '{roomInteraction.Id}' has been removed in MudGame: '{roomInteraction.Game.Name}' from '{user.Email}'");
            return new RoomInteractionResult()
            {
                RoomInteraction = roomInteraction
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
