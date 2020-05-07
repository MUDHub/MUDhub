using MUDhub.Core.Abstracts.Models.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class CharacterRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string RaceId { get; set; } = string.Empty;
        [Required]
        public string ClassId { get; set; } = string.Empty;


        public static CharacterArgs CreateArgs(CharacterRequest request)
        {
            return new CharacterArgs
            {
                Name = request.Name,
                ClassId = request.ClassId,
                RaceId = request.RaceId
            };
        }
    }
}
