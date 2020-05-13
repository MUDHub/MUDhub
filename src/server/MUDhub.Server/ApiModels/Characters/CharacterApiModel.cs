using MUDhub.Core.Models.Characters;
using MUDhub.Server.ApiModels.Muds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class CharacterApiModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerFullname { get; set; } = string.Empty;
        public string RaceName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public MudApiModel Mud { get; set; }


        public static CharacterApiModel FromCharacter(Character character)
        {
            if (character is null)
                throw new ArgumentNullException(nameof(character));

            return new CharacterApiModel
            {
                Id = character.Id,
                Name = character.Name,
                ClassName = character.Class.Name,
                RaceName = character.Race.Name,
                RoomName = character.ActualRoom.Name,
                OwnerFullname = character.Owner.Name + " " + character.Owner.Lastname,
                Mud = MudApiModel.ConvertFromMudGame(character.Game)
            };
        }
    }
}
