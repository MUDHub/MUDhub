using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts
{
    public interface INavigationService
    {
        Task<RoomResult> TryEnterRoomAsync(string characterId, string roomId);
        Task<bool> JoinWorldAsync(string mudId, string characterId);
        Task<bool> LeaveWorldAsync(string characterId);
    }
}
