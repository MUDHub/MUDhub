using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class ConnectionResult : BaseResult
    {
        public RoomConnection? RoomConnection { get; set; }
    }
}
