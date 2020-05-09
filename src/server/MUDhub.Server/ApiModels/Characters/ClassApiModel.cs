using MUDhub.Core.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class ClassApiModel
    {
        public string Description { get; set; } = string.Empty;
        public string RaceId { get; set; } = string.Empty;
        public string MudGameId { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;


        public static ClassApiModel ConvertFromCharacterClass(CharacterClass race)
        {
            return new ClassApiModel
            {
                MudGameId = race.GameId,
                Description = race.Description,
                RaceId = race.Id,
                ImageKey = race.ImageKey
            };
        }

    }
}
