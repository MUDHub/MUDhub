using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class RoomResult : BaseResult
    {
        public Room? Room { get; set; }
        public bool IsDefaultRoom { get; set; } = false;
    }
}
