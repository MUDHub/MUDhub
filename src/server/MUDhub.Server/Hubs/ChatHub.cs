using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;

namespace MUDhub.Server.Hubs
{
    public class ChatHub : Hub<IChatHubClient>, IChatHubServer
    {
        private readonly IUserManager _userManager;

        public ChatHub(IUserManager userManager)
        {
            _userManager = userManager;
        }



        public async Task SendGlobalMessage(string message)
        {
            var user = await GetUserAsync()
                .ConfigureAwait(false);
            Clients.All.ReceiveGlobalMessage(message, user.Lastname); //ToDo Später Character zurückgeben
        }

        public async Task SendPrivateMesage(string message, string targetCharacterName)
        {
            var targetUser = await GetUserAsync(targetCharacterName).ConfigureAwait(false);
            if (targetUser is null)
            {
                //Todo: handle later not with a exception.
                throw new InvalidOperationException();
            }

            var user = await GetUserAsync()
                .ConfigureAwait(false);
            Clients.User(targetUser.Id).ReceivePrivateMessage(message, user.Lastname); //TODO Später Charakter zurüchgeben
        }

        public Task SendRoomMessage(string message)
        {
            throw new NotImplementedException();
        }



        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserByIdAsync(Context.UserIdentifier)
                .ConfigureAwait(false);
            //Maybe later Save Messages.
            await Clients.Others
                .ReceiveGlobalMessage($"{user?.Lastname ?? "Unkown"} ist dem Chat beigetreten.", "Server")
                .ConfigureAwait(false);
            await base.OnConnectedAsync()
                .ConfigureAwait(false);
        }








        private async Task<User> GetUserAsync()
        {
            var user = await _userManager.GetUserByIdAsync(Context.UserIdentifier)
                .ConfigureAwait(false);
            if (user is null)
            {
                //Todo: handle later not with a exception.
                throw new InvalidOperationException();
            }
            return user;
        }
        private async Task<User> GetUserAsync(string userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                //Todo: handle later not with a exception.
                throw new InvalidOperationException();
            }
            return user;
        }
    }
}
