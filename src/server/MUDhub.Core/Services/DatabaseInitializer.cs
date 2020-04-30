using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class DatabaseInitializer : IHostedService
    {
        private readonly IUserManager _userManager;
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;
        private readonly DatabaseConfiguration _options;

        public DatabaseInitializer(IUserManager userManager, MudDbContext context,IOptions<DatabaseConfiguration> options, ILogger<DatabaseInitializer>? logger = null)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.CreateDefaultUser)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                    .ConfigureAwait(false);
                if (user != null)
                {
                    _logger?.LogWarning($"DefaultUser {_options.DefaultMudAdminEmail} already exists!");
                    return;
                }

                var registerResult =  await _userManager.RegisterUserAsync(new RegistrationArgs
                {
                    Email = _options.DefaultMudAdminEmail,
                    Password = _options.DefaultMudAdminPassword,
                    Name = "DefaultUser"
                }).ConfigureAwait(false);

                if (!registerResult.Succeeded)
                {
                    _logger?.LogError($"Something went wrong, can't create Default user: '{_options.DefaultMudAdminEmail}'.");
                    return;
                }

                var success =  await _userManager.AddRoleToUserAsync(registerResult!.User!.Id, Role.Admin)
                    .ConfigureAwait(false);
                if (success)
                {
                    //Todo: add logging message
                }
                success = await _userManager.AddRoleToUserAsync(registerResult!.User!.Id, Role.Master)
                    .ConfigureAwait(false);
                if (success)
                {
                    //Todo: add logging message
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}
