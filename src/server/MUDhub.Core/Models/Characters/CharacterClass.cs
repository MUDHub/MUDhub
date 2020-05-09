using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MUDhub.Core.Models.Characters
{
    public class CharacterClass
    {
        public CharacterClass()
            : this(Guid.NewGuid().ToString())
        {
        }

        public CharacterClass(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual MudGame Game { get; set; } = new MudGame();
        public virtual ICollection<Character> Characters { get; set; } = new Collection<Character>();
    }
}
