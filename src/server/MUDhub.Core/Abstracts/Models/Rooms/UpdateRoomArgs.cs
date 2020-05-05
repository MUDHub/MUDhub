using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class UpdateRoomArgs
    {
        public string? Description { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? ImageKey { get; set; } = null;
    }
}
