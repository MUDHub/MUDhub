using MUDhub.Core.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class CharacterApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string OwnerFullname { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }


        public static CharacterApiModel FromCharacter(Character character)
        {
            return new CharacterApiModel
            {
                Id = character.Id,
                Name = character.Name,
                Class = character.Class.Name,
                OwnerFullname = character.Owner.Name + " " +character.Owner.Lastname,
            };
        }
    }
}
