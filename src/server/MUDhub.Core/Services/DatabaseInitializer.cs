using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class DatabaseInitializer : IHostedService
    {
        private readonly IUserManager _userManager;
        private readonly IMudManager _mudManager;
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;
        private readonly DatabaseConfiguration _options;

        public DatabaseInitializer(IUserManager userManager,
                                    IMudManager mudManager,
                                    MudDbContext context,
                                    IOptions<DatabaseConfiguration> options,
                                    ILogger<DatabaseInitializer>? logger = null)
        {
            _userManager = userManager;
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
            await _mudManager.CreateMudAsync("Thors World", new MudCreationArgs
            {
                AutoRestart = true,
                Description = "It's thors 9 worlds!",
                ImageKey = "some awesome key",
                IsPublic = true,
                OwnerId = user.Id
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
                Name = "DefaultUser"
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
