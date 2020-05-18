using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Items;
using MUDhub.Server.ApiModels.Muds.RoomConnections;
using MUDhub.Server.ApiModels.Muds.Rooms;
using MUDhub.Server.Helpers;
using MUDhub.Server.Hubs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public class GameHub : Hub<IGameHubClient>, IGameHubServer
    {
        private const string SERVERNAME = "Server";

        private readonly MudDbContext _context;
        private readonly SignalRConnectionHandler _connectionHandler;
        private readonly INavigationService _navigationService;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<GameHub> _logger;

        public GameHub(MudDbContext context,
                       SignalRConnectionHandler connectionHandler,
                       INavigationService navigationService,
                       IInventoryService inventoryService,
                       ILogger<GameHub> logger)
        {
            _context = context;
            _connectionHandler = connectionHandler;
            _navigationService = navigationService;
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task SendGlobalMessage(string message)
        {
            var character = await _context.Characters.FindAsync(GetCharacterId())
                                                     .ConfigureAwait(false);
            await Clients.GroupExcept(character.Game.Id, Context.ConnectionId).ReceiveGlobalMessage(message, character.Name)
                                                  .ConfigureAwait(false);
        }

        public async Task<SendPrivateMessageResult> SendPrivateMesage(string message, string targetCharacterName)
        {
            var targetCharacter = _context.Characters.FirstOrDefault(c => c.Name == targetCharacterName);

            var targetConnid = _connectionHandler.GetConnectionId(targetCharacterName);
            if (targetConnid is null)
            {
                return new SendPrivateMessageResult
                {
                    Success = false,
                    DisplayMessage = $"{targetCharacterName} is momentan nicht online."
                };
            }
            await Clients.Client(targetConnid)
                            .ReceivePrivateMessage(message, GetCharacterName())
                            .ConfigureAwait(false);
            return new SendPrivateMessageResult
            {
                Success = true
            };
        }

        public async Task SendRoomMessage(string message)
        {
            await Clients.GroupExcept(GetCurrentRoomId(), Context.ConnectionId)
                            .ReceiveRoomMessage(message, GetCharacterName())
                            .ConfigureAwait(false);
        }

        public async Task<JoinMudGameResult> TryJoinMudGame(string characterid)
        {
            var character = (await _context.Users.FindAsync(Context.UserIdentifier)
                                                    .ConfigureAwait(false))?.Characters.FirstOrDefault(c => c.Id == characterid);
            if (character is null)
            {
                _logger?.LogWarning($"User '{Context.User.Identity.Name}' with the Id '{Context.UserIdentifier}' is not the owner from the Character with the Id '{characterid}'");
                return new JoinMudGameResult
                {
                    DisplayMessage = $"Der Benutzer '{Context.User.Identity.Name}' mit der Id '{Context.UserIdentifier}' ist nicht der Besitzer des Charakters mit der Id '{characterid}'",
                    Success = false
                };
            }
            if (character.Game.State != MudGameState.Active)
            {
                _logger?.LogWarning($"MudGame '{character.Game.Name}' with the Id '{character.Game.Id}' is not Running");
                return new JoinMudGameResult
                {
                    DisplayMessage = $"Das Mudspiel '{character.Game.Name}' mit der Id '{character.Game.Id}' läuft nicht",
                    Success = false
                };
            }

            //Note: Join stuff
            Context.Items["characterId"] = characterid;
            Context.Items["characterName"] = character.Name;
            Context.Items["currentRoomId"] = character.ActualRoom.Id;
            _connectionHandler.AddConnectionId(characterid, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, character.Game.Id).ConfigureAwait(false);
            await Groups.AddToGroupAsync(Context.ConnectionId, character.ActualRoom.Id).ConfigureAwait(false);
            await Clients.Group(character.Game.Id).ReceiveGlobalMessage($"{character.Name} hat das Spiel betreten.", SERVERNAME, true)
                                                  .ConfigureAwait(false);

            Console.WriteLine(character.ActualRoom.Id);

            return new JoinMudGameResult
            {
                Success = true,
                AreaId = character.ActualRoom.AreaId,
                Room = RoomApiModel.ConvertFromRoom(character.ActualRoom, false)
            };
        }

        public async Task<EnterRoomResult> TryEnterRoom(Direction direction, string? portalArg = null)
        {
            var character = await _context.Characters.FindAsync(GetCharacterId()).ConfigureAwait(false);
            string? targetroomid;
            if (direction == Direction.Portal)
            {
                targetroomid = GetRoomIdByPortalName(character.ActualRoom.AllConnections, character.ActualRoom, portalArg!);
            }
            else
            {
                targetroomid = GetRoomIdByDirection(character.ActualRoom.AllConnections, character.ActualRoom, direction);
            }
            if (targetroomid is null)
            {
                return new EnterRoomResult
                {
                    Success = false,
                    ErrorType = NavigationErrorType.RoomsAreNotConnected
                };
            }

            var result = await _navigationService.TryEnterRoomAsync(GetCharacterId(), targetroomid)
                                                    .ConfigureAwait(false);
            if (result.Success)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetCurrentRoomId())
                            .ConfigureAwait(false);

                Context.Items["currentRoomId"] = result.ActiveRoom?.Id;

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetCurrentRoomId())
                            .ConfigureAwait(false);

            }

            return EnterRoomResult.ConvertFromNavigationResult(result);
        }

        public async Task<TransferItemResult> TryTransferItem(string itemName, ItemTransferMethode reason)
        {
            var targetinventoryId = reason switch
            {
                ItemTransferMethode.Drop => (await _context.Rooms.FindAsync(GetCurrentRoomId())
                                                                     .ConfigureAwait(false))?
                                                                        .Inventory.Id,
                ItemTransferMethode.Pickup => (await _context.Characters.FindAsync(GetCharacterId())
                                                                     .ConfigureAwait(false))?
                                                                        .Inventory.Id,
                _ => throw new NotSupportedException()
            };

            if (targetinventoryId is null)
            {
                return new TransferItemResult
                {
                    Success = false,
                    ErrorMessage = "No inventoryId found, this should never happen! It's a critical bug on the server."
                };
            }

            var soruceinventoryId = reason switch
            {
                ItemTransferMethode.Drop => (await _context.Characters.FindAsync(GetCharacterId())
                                                                     .ConfigureAwait(false))?
                                                                        .Inventory.Id,
                ItemTransferMethode.Pickup => (await _context.Rooms.FindAsync(GetCurrentRoomId())
                                                                     .ConfigureAwait(false))?
                                                                        .Inventory.Id,
                _ => throw new NotSupportedException()
            };

            if (soruceinventoryId is null)
            {
                return new TransferItemResult
                {
                    Success = false,
                    ErrorMessage = "No inventoryId found, this should never happen! It's a critical bug on the server."
                };
            }
            var itemId = reason switch
            {
                ItemTransferMethode.Drop => (await _context.Characters.FindAsync(GetCharacterId())
                                                                     .ConfigureAwait(false))?
                                                                        .Inventory.ItemInstances.FirstOrDefault(i => i.Item.Name == itemName)?.Id,
                ItemTransferMethode.Pickup => (await _context.Rooms.FindAsync(GetCurrentRoomId())
                                                                     .ConfigureAwait(false))?
                                                                        .Inventory.ItemInstances.FirstOrDefault(i => i.Item.Name == itemName)?.Id,
                _ => throw new NotSupportedException()
            };

            if (itemId is null)
            {
                return new TransferItemResult
                {
                    Success = false,
                    ErrorMessage = $"No Item found in the inventory '{targetinventoryId}'",
                    DisplayMessage = reason switch
                    {
                        ItemTransferMethode.Drop => $"Es gibt kein Gegenstand '{itemName}' in deinem Inventar.",
                        ItemTransferMethode.Pickup => $"Es gibt kein Gegenstand '{itemName}' im aktuellen Raum.",
                        _ => throw new NotSupportedException()
                    }
                };
            }

            var result = await _inventoryService.TransferItemAsync(itemId, targetinventoryId, soruceinventoryId).ConfigureAwait(false);
            if (result.Success)
            {
                return new TransferItemResult
                {
                    Success = true,
                    DisplayMessage = reason switch
                    {
                        ItemTransferMethode.Drop => $"Gegenstand '{itemName}' weggeworfen.",
                        ItemTransferMethode.Pickup => $"Gegenstand '{itemName}' aufgeoben und in das Inventar gepackt.",
                        _ => throw new NotSupportedException()
                    }
                };
            }
            else
            {
                return new TransferItemResult
                {
                    Success = false,
                    DisplayMessage = result.DisplayMessage,
                    ErrorMessage = result.Errormessage
                };
            }

        }

        public async Task<IEnumerable<RoomConnectionApiModel>> GetRoomConnections()
        {
            var character = await _context.Characters.FindAsync(GetCharacterId()).ConfigureAwait(false);
            if (character is null)
            {
                //Note: Should never happen
                throw new ArgumentException();
            }
            return character.ActualRoom.AllConnections.Select(rc => RoomConnectionApiModel.ConvertFromRoomConnection(rc));
        }

        public async Task<InventoryResult> GetInventory(bool getActualRoomInventory)
        {

            if (getActualRoomInventory)
            {
                return new InventoryResult
                {
                    Success = true,
                    Items = (await _context.Rooms.FindAsync(GetCurrentRoomId())
                                                .ConfigureAwait(false))?
                                                  .Inventory
                                                  .ItemInstances
                                                  .Select(ii => ItemInstanceApiModel.ConvertFromItemInstance(ii))
                                                  ?? Enumerable.Empty<ItemInstanceApiModel>()
                };
            }
            else
            {
                return new InventoryResult
                {
                    Success = true,
                    Items = (await _context.Characters.FindAsync(GetCharacterId())
                                                 .ConfigureAwait(false))?
                                                   .Inventory
                                                   .ItemInstances
                                                   .Select(ii => ItemInstanceApiModel.ConvertFromItemInstance(ii))
                                                   ?? Enumerable.Empty<ItemInstanceApiModel>()
                };
            }

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var character = await _context.Characters.FindAsync(GetCharacterId()).ConfigureAwait(false);
            _logger.LogInformation($"Character {character?.Name} ({character?.Id}) has left the game");
            if (!(character is null))
            {
                await Clients.Group(character.Game.Id).ReceiveGlobalMessage($"{character.Name} hat das Spiel verlassen.", SERVERNAME, true)
                                                    .ConfigureAwait(false);
                _connectionHandler.RemoveConnectionId(character.Id);
            }
        }

        private string GetCharacterId() => (Context.Items["characterId"] as string)!;
        private string GetCharacterName() => (Context.Items["characterName"] as string)!;
        private string GetCurrentRoomId() => (Context.Items["currentRoomId"] as string)!;

        private static string? GetRoomIdByDirection(IEnumerable<RoomConnection> connections, Room currentRoom, Direction direction)
        {
            foreach (var connection in connections)
            {
                if (connection.Room1.AreaId == connection.Room2.AreaId)
                {
                    int xDif, yDif;
                    if (currentRoom.Id == connection.Room1.Id)
                    {
                        xDif = connection.Room1.X - connection.Room2.X;
                        yDif = connection.Room1.Y - connection.Room2.Y;
                    }
                    else
                    {
                        xDif = connection.Room2.X - connection.Room1.X;
                        yDif = connection.Room2.Y - connection.Room1.Y;
                    }

                    var id = (xDif, yDif, direction) switch
                    {
                        (0, -1, Direction.South) => currentRoom.Id == connection.Room1Id ? connection.Room2Id : connection.Room1Id,
                        (0, 1, Direction.North) => currentRoom.Id == connection.Room1Id ? connection.Room2Id : connection.Room1Id,
                        (-1, 0, Direction.East) => currentRoom.Id == connection.Room1Id ? connection.Room2Id : connection.Room1Id,
                        (1, 0, Direction.West) => currentRoom.Id == connection.Room1Id ? connection.Room2Id : connection.Room1Id,
                        _ => null
                    };
                    if (!(id is null))
                    {
                        return id;
                    }
                }
            }
            return null;
        }

        private static string? GetRoomIdByPortalName(IEnumerable<RoomConnection> connections, Room currentRoom, string portalname)
        {
            foreach (var connection in connections)
            {
                if (connection.Room1.AreaId != connection.Room2.AreaId)
                {
                    if (connection.Room1Id == currentRoom.Id && connection.Room2.Name == portalname)
                    {
                        return connection.Room2Id;
                    }
                    if (connection.Room2Id == currentRoom.Id && connection.Room1.Name == portalname)
                    {
                        return connection.Room1Id;
                    }
                }
            }
            return null;
        }

    }
}
