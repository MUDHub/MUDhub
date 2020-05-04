using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

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

        public string Name { get; set; }

        public MudGame Game { get; set; }

        public User Owner { get; set; }

        public CharacterRace Race { get; set; }

        public CharacterClass Class { get; set; }

        //"LiveUpdates"

        public int Health { get; set; }
        public int Starvation { get; set; }


        public ICollection<CharacterBoost> ActiveBoosts { get; set; } = new Collection<CharacterBoost>();
        //Todo: Later after room implementation
        //public string ActiveRoomId { get; set; }
    }
}
