using MUDhub.Core.Models.Rooms;
using System;

namespace MUDhub.Core.Models.Connections
{
    public class RoomConnection
    {
        public RoomConnection()
        {
            Id = Guid.NewGuid().ToString();
        }

        public RoomConnection(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public string? Description { get; set; } = string.Empty;
        public string Room1Id { get; set; } = string.Empty;
        public virtual Room Room1 { get; set; } = null!;
        public string Room2Id { get; set; } = string.Empty;
        public virtual Room Room2 { get; set; } = null!;

        public LockType LockType { get; set; } = LockType.NoLock;

        public string LockDescription { get; set; } = string.Empty;
        public string LockAssociatedId { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
    }
}
