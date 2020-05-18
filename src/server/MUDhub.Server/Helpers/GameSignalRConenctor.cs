using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Helper;
using MUDhub.Server.Hubs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MUDhub.Server.Helpers
{
    public class GameSignalRConenctor : IHostedService
    {
        public GameSignalRConenctor(IHubContext<GameHub,IGameHubClient> context, GameActiveHelper helper)
        {
            helper.MudGameStopped += game =>
            {
                context.Clients.Group(game.Id).KickFromServer();
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
