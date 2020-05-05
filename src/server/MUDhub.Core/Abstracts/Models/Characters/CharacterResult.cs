using MUDhub.Core.Models.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Characters
{
    public class CharacterResult : BaseResult
    {
        public Character? Character { get; set; }
    }
}
