using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class MudGameInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<MudGameInitializer>? _logger;

        public MudGameInitializer(IServiceScopeFactory scopeFactory,
                                    IHostApplicationLifetime lifetime,
                                    ILogger<MudGameInitializer>? logger = null)
        {
            _scopeFactory = scopeFactory;
            _lifetime = lifetime;
            _logger = logger;
            
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _lifetime.ApplicationStarted.Register(() =>
            {
                using var scopedServices = _scopeFactory.CreateScope();
                var services = scopedServices.ServiceProvider;
                var context = services.GetRequiredService<MudDbContext>();
                foreach (var game in context.MudGames)
                {
                    if (game.AutoRestart)
                    {
                        game.State = MudGameState.Active;
                    }
                }
            });
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scopedServices = _scopeFactory.CreateScope();
            var services = scopedServices.ServiceProvider;
            var context = services.GetRequiredService<MudDbContext>();
            await context.MudGames.ForEachAsync(mg => mg.State = MudGameState.InActive).ConfigureAwait(false);
        }
    }
}
