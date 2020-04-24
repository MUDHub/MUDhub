using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models
{
    public class MudGame
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? ImageKey { get; set; }

        public bool IsPublic { get; set; }

        public MudGameState State { get; set; }

        public bool AutoRestart { get; set; }

        //Todo: Add Navigation properties
    }
}
