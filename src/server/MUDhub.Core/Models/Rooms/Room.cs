using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MUDhub.Core.Models.Rooms
{
    public class Room
    {
        public Room()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Room(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<RoomConnection> Connections1 { get; set; } = new Collection<RoomConnection>();
        public virtual ICollection<RoomConnection> Connections2 { get; set; } = new Collection<RoomConnection>();
        public IEnumerable<RoomConnection> AllConnections 
            => Enumerable.Concat(Connections1, Connections2);
        public string AreaId { get; set; } = string.Empty;
        public virtual Area Area { get; set; } = null!;
        public virtual ICollection<RoomInteraction> Interactions { get; set; } = new Collection<RoomInteraction>();
        public string ImageKey { get; set; } = string.Empty;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public string GameId { get; set; } = string.Empty;
        public virtual MudGame Game { get; set; } = null!;
        public bool IsDefaultRoom { get; set; } = false;
        public virtual ICollection<Character> Characters { get; set; } = new Collection<Character>();
        public string InventoryId { get; set; } = string.Empty;
        public virtual Inventory Inventory { get; set; } = null!;
    }
}
