using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Helper
{
    public class GameActiveHelper
    {
        public event Action<MudGame> MudGameStopped;

        internal void GameStopped(MudGame game)
        {
            MudGameStopped?.Invoke(game);
        }
    }
}
