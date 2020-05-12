using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public interface IGameHubClient
    {
        Task ReceiveMudMasterMessage(object o);

        Task ReceiveGameMessage(string message);
        Task ReceiveGlobalMessage(string message, string caller, bool serverMessage = false);
        Task ReceiveRoomMessage(string message, string caller);
        Task ReceivePrivateMessage(string mssage, string caller);
    }
}
