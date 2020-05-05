using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models.Rooms
{
    public class RoomConnection
    {

        public RoomConnection(string roomConnectionId)
        {
            Id = roomConnectionId;
        }

        public string Id { get; }

        public string Description { get; set; } = string.Empty;
        public Room Room1 { get; set; } = new Room();
        public Room Room2 { get; set; } = new Room();

        //Todo: Was ist der Default-Wert für diese Property?
        public ConnectionDirection Direction { get; set; } = ConnectionDirection.North;

        //Todo: Was ist der Default-Wert für diese Property?
        public LockType LockType { get; set; } = LockType.NoLock;

        public string LockDescription { get; set; } = string.Empty;
        public string LockAssociatedId { get; set; } = string.Empty;
    }
}
