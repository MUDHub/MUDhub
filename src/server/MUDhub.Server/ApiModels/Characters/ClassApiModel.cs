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
        public string ClassId { get; set; } = string.Empty;
        public string MudGameId { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;


        public static ClassApiModel ConvertFromCharacterClass(CharacterClass classes)
        {
            if (classes is null)
                throw new ArgumentNullException(nameof(classes));

            return new ClassApiModel
            {
                MudGameId = classes.GameId,
                Description = classes.Description,
                ClassId = classes.Id,
                ImageKey = classes.ImageKey,
                Name = classes.Name
            };
        }

    }
}
