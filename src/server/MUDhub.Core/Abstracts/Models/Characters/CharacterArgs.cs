using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Characters
{
    public class CharacterArgs
    {
        public string Name { get; set; } = string.Empty;
        public string RaceId { get; set; } = string.Empty;
        public string ClassId { get; set; } = string.Empty;

    }
}
