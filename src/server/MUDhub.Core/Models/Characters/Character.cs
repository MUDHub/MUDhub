using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MUDhub.Core.Models.Characters
{
    public class Character
    {
        public Character()
            : this(Guid.NewGuid().ToString())
        {
        }

        public Character(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; } = string.Empty;
        public virtual MudGame Game { get; set; } = null!;
        public virtual User Owner { get; set; } = null!;

        public virtual CharacterRace Race { get; set; }  = null!;
        public virtual CharacterClass Class { get; set; } = null!;
        public virtual Room ActualRoom { get; set; } = null!;
        public virtual Inventory Inventory { get; set; } = null!;

        //"LiveUpdates"

        public int Health { get; set; }
        public int Starvation { get; set; }


        public virtual ICollection<CharacterBoost> ActiveBoosts { get; set; } = new Collection<CharacterBoost>();
        //Todo: Later after room implementation
        //public string ActiveRoomId { get; set; }
    }
}
