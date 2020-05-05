using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts
{
    public interface IAreaManager
    {
        Task<AreaResult> CreateAreaAsync(string userId, string mudId, AreaArgs args);
        Task<AreaResult> RemoveAreaAsync(string userId, string areaId);
        Task<AreaResult> UpdateAreaAsync(string userId, string areaId);
        Task<RoomResult> CreateRoomAsync(string userId, string areaId, RoomArgs args);
        Task<RoomResult> UpdateRoomAsync(string userId, string roomId);
        Task<RoomResult> RemoveRoomAsync(string userId, string roomId);
        //Room GetRoomById(string roomId);
        //Area GetAreaById(string areaId);
        //Task<IEnumerable<RoomConnection>> GetRoomConnections(string roomId);
    }
}
