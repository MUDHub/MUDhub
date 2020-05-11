using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models.Characters
{
    public class CharacterBoost
    {
        public CharacterBoost()
            : this(Guid.NewGuid().ToString())
        {
        }

        public CharacterBoost(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; } = string.Empty;//Todo: mabye later remove, at this point no reason for Boostname.
        public BoostType Type { get; set; }
        public int Value { get; set; }

        //Todo: maybe add time stamp or remove all boosts after restart.
        public virtual Character Character { get; set; } = new Character();
    }
}
