﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models.Muds
{
    public class MudGame
    {
        public MudGame()
        {
            JoinRequests = new List<MudJoinRequest>();
        }

        public MudGame(string id)
            : this()
        {
            Id = id;
        }

        public string Id { get; } = Guid.NewGuid().ToString();

        public string Description { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? ImageKey { get; set; }

        public bool IsPublic { get; set; }

        //Todo: Later move the state to something like GameService, for saving the state in Memory.
        public MudGameState State { get; set; }

        public bool AutoRestart { get; set; }

        //Todo: Add Navigation properties

        public ICollection<MudJoinRequest> JoinRequests { get; set; }
    }
}
