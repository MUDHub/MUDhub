using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class RoomArgs
    {
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;
    }
}
