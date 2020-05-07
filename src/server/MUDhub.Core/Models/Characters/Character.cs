using MUDhub.Core.Models.Muds;
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

        public MudGame Game { get; set; } = new MudGame();

        public User Owner { get; set; } = new User();

        public CharacterRace Race { get; set; } = new CharacterRace();

        public CharacterClass Class { get; set; } = new CharacterClass();

        //"LiveUpdates"

        public int Health { get; set; }
        public int Starvation { get; set; }


        public ICollection<CharacterBoost> ActiveBoosts { get; set; } = new Collection<CharacterBoost>();
        //Todo: Later after room implementation
        //public string ActiveRoomId { get; set; }
    }
}
