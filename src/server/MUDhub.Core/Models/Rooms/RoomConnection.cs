using System;

namespace MUDhub.Core.Models.Rooms
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

        public string Description { get; set; } = string.Empty;
        public string Room1Id { get; set; } = string.Empty;
        public Room Room1 { get; set; } = new Room();
        public string Room2Id { get; set; } = string.Empty;
        public Room Room2 { get; set; } = new Room();

        public LockType LockType { get; set; } = LockType.NoLock;

        public string LockDescription { get; set; } = string.Empty;
        public string LockAssociatedId { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
    }
}
