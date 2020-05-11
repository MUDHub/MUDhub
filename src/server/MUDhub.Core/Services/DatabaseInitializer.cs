using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Abstracts.Models.Areas;
using MUDhub.Core.Abstracts.Models.Connections;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class DatabaseInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger? _logger;
        private readonly DatabaseConfiguration _options;

        public DatabaseInitializer(IServiceScopeFactory scopeFactory,
                                    IOptions<DatabaseConfiguration> options,
                                    ILogger<DatabaseInitializer>? logger = null)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _scopeFactory.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<MudDbContext>();
            var mudManager = serviceScope.ServiceProvider.GetRequiredService<IMudManager>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<IUserManager>();
            var areaManager = serviceScope.ServiceProvider.GetRequiredService<IAreaManager>();

            if (_options.DeleteDatabase)
            {
                _logger?.LogWarning("Database will be deleted.");
                context.Database.EnsureDeleted();
            }
            if (context.Database.IsSqlite())
            {

                _logger?.LogWarning("The Server may has a new Data schema Version, sqlite don't support some Migration operations. " +
                                    "The old database must be deleted and a will be automatically created. " +
                                    $"Delete database manually or set '{nameof(DatabaseConfiguration.DeleteDatabase)}' option to 'true'.");
                context.Database.EnsureCreated();
            }
            else
            {
                context.Database.Migrate();
            }

            var userExists = true;
            if (_options.CreateDefaultUser)
            {
                userExists = await CreateDefaultUserAsnyc(context, userManager)
                         .ConfigureAwait(false);
            }

            if (_options.CreateDefaultMudData && userExists)
            {
                await CreateDefaultMudDataAsync(context, mudManager, areaManager)
                        .ConfigureAwait(false);
            }
        }

        private async Task CreateDefaultMudDataAsync(MudDbContext context, IMudManager mudManager, IAreaManager areaManager)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                                        .ConfigureAwait(false);
            var resultGame = await mudManager.CreateMudAsync("Thors World", new MudCreationArgs
            {
                AutoRestart = true,
                Description = "It's thors 9 worlds!",
                ImageKey = "some awesome key",
                IsPublic = true,
                OwnerId = user.Id
            }).ConfigureAwait(false);

            var resultArea = await areaManager.CreateAreaAsync(user.Id, resultGame!.Id, new AreaArgs()
            {
                Name = "First Etage",
                Description = "Nice View"
            }).ConfigureAwait(false);

            var resultRoom1 = await areaManager.CreateRoomAsync(user.Id, resultArea.Area.Id, new RoomArgs()
            {
                Name = "Dinner Room",
                Description = "Yummy Food",
                IsDefaultRoom = true,
                X = 1,
                Y = 1
            }).ConfigureAwait(false);

            var resultRoom2 = await areaManager.CreateRoomAsync(user.Id, resultArea.Area.Id, new RoomArgs()
            {
                Name = "Sleeping Room",
                Description = "Naughty Things to see",
                IsDefaultRoom = false,
                X = 2,
                Y = 1
            }).ConfigureAwait(false);

            var resultConnection = await areaManager.CreateConnectionAsync(user.Id, resultRoom1.Room.Id, resultRoom2.Room.Id, new RoomConnectionsArgs()
            {
                Description = "From Dinner to Sleep",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
        }

        private async Task<bool> CreateDefaultUserAsnyc(MudDbContext context, IUserManager userManager)
        {

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                .ConfigureAwait(false);
            if (user != null)
            {
                _logger?.LogWarning($"DefaultUser {_options.DefaultMudAdminEmail} already exists!");
                return true;
            }

            var registerResult = await userManager.RegisterUserAsync(new RegistrationUserArgs
            {
                Email = _options.DefaultMudAdminEmail,
                Password = _options.DefaultMudAdminPassword,
                Lastname = "",
                Firstname = "DefaultUser"
            }).ConfigureAwait(false);

            if (!registerResult.Success)
            {
                _logger?.LogError(registerResult.Errormessage);
                return false;
            }

            var success = await userManager.AddRoleToUserAsync(registerResult!.User!.Id, Roles.Admin)
                .ConfigureAwait(false);
            if (success)
            {
                _logger?.LogInformation("The role Admin was added to the user");
            }
            success = await userManager.AddRoleToUserAsync(registerResult!.User!.Id, Roles.Master)
                .ConfigureAwait(false);
            if (success)
            {
                _logger?.LogInformation("The role Master was added to the user");
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
