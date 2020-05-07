using Microsoft.EntityFrameworkCore;
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
    public class DatabaseInitializer : IHostedService
    {
        private readonly IUserManager _userManager;
        private readonly IMudManager _mudManager;
        private readonly IAreaManager _areaManager;
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;
        private readonly DatabaseConfiguration _options;

        public DatabaseInitializer(IUserManager userManager,
                                    IMudManager mudManager,
                                    IAreaManager areaManager,
                                    MudDbContext context,
                                    IOptions<DatabaseConfiguration> options,
                                    ILogger<DatabaseInitializer>? logger = null)
        {
            _userManager = userManager;
            _areaManager = areaManager;
            _mudManager = mudManager;
            _context = context;
            _logger = logger;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var userExists = true;
            if (_options.CreateDefaultUser)
            {
                userExists = await CreateDefaultUserAsnyc()
                         .ConfigureAwait(false);
            }

            if (_options.CreateDefaultMudData && userExists)
            {
                await CreateDefaultMudDataAsync()
                        .ConfigureAwait(false);
            }
        }

        private async Task CreateDefaultMudDataAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                                        .ConfigureAwait(false);
            var resultGame = await _mudManager.CreateMudAsync("Thors World", new MudCreationArgs
            {
                AutoRestart = true,
                Description = "It's thors 9 worlds!",
                ImageKey = "some awesome key",
                IsPublic = true,
                OwnerId = user.Id
            }).ConfigureAwait(false);

            var resultArea = await _areaManager.CreateAreaAsync(user.Id, resultGame!.Id, new AreaArgs()
            {
                Name = "First Etage",
                Description = "Nice View"
            }).ConfigureAwait(false);

            var resultRoom1 = await _areaManager.CreateRoomAsync(user.Id, resultArea.Area.Id, new RoomArgs()
            {
                Name = "Dinner Room",
                Description = "Yummy Food",
                IsDefaultRoom = true,
                X = 1,
                Y = 1
            }).ConfigureAwait(false);

            var resultRoom2 = await _areaManager.CreateRoomAsync(user.Id, resultArea.Area.Id, new RoomArgs()
            {
                Name = "Sleeping Room",
                Description = "Naughty Things to see",
                IsDefaultRoom = false,
                X = 2,
                Y = 1
            }).ConfigureAwait(false);

            var resultConnection = await _areaManager.CreateConnectionAsync(user.Id, resultRoom1.Room.Id, resultRoom2.Room.Id, new RoomConnectionsArgs()
            {
                Description = "From Dinner to Sleep",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
        }

        private async Task<bool> CreateDefaultUserAsnyc()
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                .ConfigureAwait(false);
            if (user != null)
            {
                _logger?.LogWarning($"DefaultUser {_options.DefaultMudAdminEmail} already exists!");
                return true;
            }

            var registerResult = await _userManager.RegisterUserAsync(new RegistrationUserArgs
            {
                Email = _options.DefaultMudAdminEmail,
                Password = _options.DefaultMudAdminPassword,
                Lastname = "",
                Firstname = "DefaultUser"
            }).ConfigureAwait(false);

            if (!registerResult.Succeeded)
            {
                _logger?.LogError($"Something went wrong, can't create Default user: '{_options.DefaultMudAdminEmail}'.");
                return false;
            }

            var success = await _userManager.AddRoleToUserAsync(registerResult!.User!.Id, Roles.Admin)
                .ConfigureAwait(false);
            if (success)
            {
                //Todo: add logging message
            }
            success = await _userManager.AddRoleToUserAsync(registerResult!.User!.Id, Roles.Master)
                .ConfigureAwait(false);
            if (success)
            {
                //Todo: add logging 
            }
            return true;

        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
