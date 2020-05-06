using System;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Server.ApiModels.Muds.RoomConnections
{
    public class RoomConnectionApiModel
    {

        public static RoomConnectionApiModel ConvertFromRoomConnection(RoomConnection connection)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            return new RoomConnectionApiModel()
            {

            };
        }
    }
}
