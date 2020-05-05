using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class LockArgs
    {
        public LockType LockType { get; set; }
        public string LockDescription { get; set; }
        public string LockAssociatedId { get; set; }
    }
}
