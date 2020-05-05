using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Rooms;

namespace MUDhub.Core.Services
{
    public class AreaManager : IAreaManager
    {
        public Task<AreaResult> CreateAreaAsync(string userId, string mudId, AreaArgs args)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResult> CreateRoomAsync(string userId, string areaId, RoomArgs args)
        {
            throw new NotImplementedException();
        }

        public Task<AreaResult> RemoveAreaAsync(string userId, string areaId)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResult> RemoveRoomAsync(string userId, string roomId)
        {
            throw new NotImplementedException();
        }

        public Task<AreaResult> UpdateAreaAsync(string userId, string areaId)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResult> UpdateRoomAsync(string userId, string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
