using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MUDhub.Core.Models.Rooms
{
    public class Room
    {
        public Room()
        {
            
        }

        public Room(string roomId)
        {
            Id = roomId;
        }

        public string Id { get; }

        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<RoomConnection> Connections { get; set; } = new Collection<RoomConnection>();
        public Area Area { get; set; } = new Area();
        public ICollection<RoomInteraction> Interactions { get; set; } = new Collection<RoomInteraction>();
        public string ImageKey { get; set; } = string.Empty;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

    }
}
