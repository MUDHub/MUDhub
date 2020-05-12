using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.DynamicProxy.Generators.Emitters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Characters;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Users;
using MUDhub.Core.Services;
using MUDhub.Server.Hubs.Models;

namespace MUDhub.Server.Hubs
{
    public class GameHub : Hub<IGameHubClient>, IGameHubServer
    {
        private const string SERVERNAME = "Server";

        private readonly MudDbContext _context;
        private readonly ICharacterManager _manager;
        private readonly ILogger<GameHub> _logger;

        public GameHub(MudDbContext context, ICharacterManager manager, ILogger<GameHub> logger)
        {
            _context = context;
            _manager = manager;
            _logger = logger;
        }

        public Task SendGlobalMessage(string message)
        {
            throw new NotImplementedException();
        }

        public Task<SendPrivateMessageResult> SendPrivateMesage(string message, string targetCharacterName)
        {
            throw new NotImplementedException();
        }

        public Task SendRoomMessage(string message)
        {
            throw new NotImplementedException();
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
            await Clients.Group(character.Game.Id).ReceiveGlobalMessage($"Charakter: {character.Name} hat das Spiel betreten.", SERVERNAME, true)
                                                  .ConfigureAwait(false);
            await Groups.AddToGroupAsync(Context.ConnectionId, character.Game.Id).ConfigureAwait(false);
            await Groups.AddToGroupAsync(Context.ConnectionId, character.ActualRoom.Id).ConfigureAwait(false);

            return new JoinMudGameResult
            {
                Success = true
            };
        }

        public async Task<JoinRoomResult> TryJoinRoom(string roomid)
        {

            throw new NotImplementedException();
        }

        public Task<TransferItemResult> TryTransferItem(string itemid, string targetid)
        {
            throw new NotImplementedException();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var character = await _context.Characters.FindAsync(GetCharacterId()).ConfigureAwait(false);
            if (!(character is null))
            {
                await Clients.Group(character.Game.Id).ReceiveGlobalMessage($"Charakter: {character.Name} hat das Spiel verlassen.", SERVERNAME, true)
                                                    .ConfigureAwait(false);
            }
        }

        private string GetCharacterId() => (Context.Items["characterId"] as string)!;
    }
}
