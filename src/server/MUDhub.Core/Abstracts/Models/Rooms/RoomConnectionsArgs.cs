using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class RoomConnectionsArgs
    {
        public LockArgs LockArgs { get; set; } = new LockArgs();
        public string Description { get; set; } = string.Empty;
    }
}
