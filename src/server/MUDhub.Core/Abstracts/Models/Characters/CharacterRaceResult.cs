using Microsoft.EntityFrameworkCore.Storage;
using MUDhub.Core.Models.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Characters
{
    public class CharacterRaceResult : BaseResult
    {
        public CharacterRace? Race { get; set; }
    }
}
