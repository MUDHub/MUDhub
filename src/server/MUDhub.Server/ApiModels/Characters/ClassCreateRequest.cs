using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MUDhub.Core.Abstracts.Models.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class ClassCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;

        public static CharacterClassArgs ConvertToCharacterClassArgs(ClassCreateRequest request)
        {
            return new CharacterClassArgs
            {
                Desctiption = request.Description,
                ImageKey = request.ImageKey,
                Name = request.Name
            };
        }
    }
}
