using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Services
{
    public class NavigationService : INavigationService
    {
        public Task<bool> JoinWorldAsync(string mudId, string characterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LeaveWorldAsync(string characterId)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResult> TryEnterRoomAsync(string characterId, string roomId)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResult> TryEnterRoomAsync(string characterId, ConnectionDirection direction)
        {
            throw new NotImplementedException();
        }
    }
}
