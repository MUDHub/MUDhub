using MUDhub.Core.Abstracts.Models.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class RaceCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImageKey { get; set; } = string.Empty;

        public static CharacterRaceArgs ConvertToCharacterRaceArgs(RaceCreateRequest request)
        {
            return new CharacterRaceArgs
            {
                Desctiption = request.Description,
                ImageKey = request.ImageKey,
                Name = request.Name
            };
        }
    }
}
