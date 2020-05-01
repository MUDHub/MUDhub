using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public interface IChatHubServer
    {
        Task SendGlobalMessage(string message);
        Task SendRoomMessage(string message);
        Task SendPrivateMesage(string message, string targetCharacterName);
    }
}
