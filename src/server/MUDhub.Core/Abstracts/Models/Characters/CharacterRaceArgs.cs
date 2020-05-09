using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Characters
{
    public class CharacterRaceArgs
    {
        public string Name { get; set; } = string.Empty;
        public string Desctiption { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;
    }
}
