using MUDhub.Core.Abstracts.Models.Areas;
using MUDhub.Core.Abstracts.Models.Connections;
using MUDhub.Core.Abstracts.Models.Rooms;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IAreaManager
    {
        Task<AreaResult> CreateAreaAsync(string userId, string mudId, AreaArgs args);
        Task<AreaResult> RemoveAreaAsync(string userId, string areaId);
        Task<AreaResult> UpdateAreaAsync(string userId, string areaId, UpdateAreaArgs args);
        Task<RoomResult> CreateRoomAsync(string userId, string areaId, RoomArgs args);
        Task<RoomResult> UpdateRoomAsync(string userId, string roomId, UpdateRoomArgs args);
        Task<RoomResult> RemoveRoomAsync(string userId, string roomId);

        Task<ConnectionResult> CreateConnectionAsync(string userId, string room1Id, string room2Id, RoomConnectionsArgs args);
        Task<ConnectionResult> UpdateConnectionAsync(string userId, string connectionId, UpdateRoomConnectionsArgs args);
        Task<ConnectionResult> RemoveConnectionAsync(string userId, string connectionId);

        //Todo: Brauchen wir diese Propertys?
        //Room GetRoomById(string roomId);
        //Area GetAreaById(string areaId);
        //Task<IEnumerable<RoomConnection>> GetRoomConnections(string roomId);
    }
}
