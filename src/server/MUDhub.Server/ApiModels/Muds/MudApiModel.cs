﻿using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using MUDhub.Server.ApiModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Muds
{
    public class MudApiModel
    {

        public string MudId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public bool AutoRestart { get; set; } = false;
        public UserApiModel Owner { get; set; } = new UserApiModel();

        public static MudApiModel ConvertFromMudGame(MudGame game)
        {
            return new MudApiModel
            {
                Description = game.Description,
                IsPublic = game.IsPublic,
                MudId = game.Id,
                Name = game.Name,
                Owner = UserApiModel.CreateFromUser(game.Owner),
                AutoRestart = game.AutoRestart
            };
        }
        
    }
}
