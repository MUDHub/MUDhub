using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public interface IChatHubClient
    {
        Task ReceiveGlobalMessage(string message, string caller);
        Task ReceiveRoomMessage(string message, string caller);
        Task ReceivePrivateMessage(string mssage, string caller);
    }
}
