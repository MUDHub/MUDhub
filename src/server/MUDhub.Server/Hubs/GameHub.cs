using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Update;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using MUDhub.Server.Hubs.Models;

namespace MUDhub.Server.Hubs
{
    public class GameHub : Hub<IGameHubClient>, IGameHubServer
    {
        public GameHub()
        {
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

        public Task<JoinMudGameResult> TryJoinMudGame(string characterid)
        {
            throw new NotImplementedException();
        }

        public Task<JoinRoomResult> TryJoinRoom(string roomid)
        {
            throw new NotImplementedException();
        }

        public Task<TransferItemResult> TryTransferItem(string itemid, string targetid)
        {
            throw new NotImplementedException();
        }
    }
}
