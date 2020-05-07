using MUDhub.Core.Models.Connections;
using System;

namespace MUDhub.Server.ApiModels.Muds.RoomConnections
{
    public class RoomConnectionApiModel
    {
        public string Description { get; set; } = string.Empty;

        public LockType LockType { get; set; } = LockType.NoLock;

        public string LockDescription { get; set; } = string.Empty;
        public string LockAssociatedId { get; set; } = string.Empty;
        public string Room1Id { get; set; } = string.Empty;
        public string Room2Id { get; set; } = string.Empty;

        public static RoomConnectionApiModel ConvertFromRoomConnection(RoomConnection connection)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            return new RoomConnectionApiModel()
            {
                Description = connection.Description,
                LockAssociatedId = connection.LockAssociatedId,
                LockDescription = connection.LockDescription,
                LockType = connection.LockType,
                Room1Id = connection.Room1Id,
                Room2Id = connection.Room2Id
            };
        }
    }
}
