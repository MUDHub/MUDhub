using MUDhub.Core.Abstracts;
using System;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Configurations;
using MUDhub.Core.Helper;
using MUDhub.Core.Abstracts.Models;

namespace MUDhub.Core.Services
{
    internal class LoginService : ILoginService
    {
        private readonly MudDbContext _dbContext;
        private readonly string _tokensecret;
        private readonly ILogger? _logger;
        public LoginService(MudDbContext context, IOptions<ServerConfiguration> options, ILogger<LoginService>? logger = null)
            : this(context, options?.Value ?? throw new ArgumentNullException(nameof(options)), logger)
        {
        }

        internal LoginService(MudDbContext context, ServerConfiguration options, ILogger<LoginService>? logger = null)
        {
            _dbContext = context;
            _tokensecret = options.TokenSecret;
            _logger = logger;
        }

        /// <summary>
        /// The user can login with email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginUserAsync(string email, string password)
        {
            var norm = UserHelpers.ToNormelizedEmail(email);
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.NormalizedEmail == norm).ConfigureAwait(false);
            var passwordHash = UserHelpers.CreatePasswordHash(password);
            if (user is null)
            {
                var message = $"No user was found with the email {email}";
                _logger?.LogWarning(message);
                return new LoginResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Es wurd kein User mit der Email-Adresse: '{email}'."
                };
            }

            if (passwordHash != user.PasswordHash)
            {
                var message = $"The Password of '{user.Name} {user.Lastname}' is wrong";
                _logger?.LogWarning(message);
                return new LoginResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Das Passwort von '{user.Name} {user.Lastname}' ist nicht korrekt."
                };
            }
            _logger?.LogInformation($"The User '{user.Name} {user.Lastname}' was logged in.");
            return new LoginResult()
            {
                Token = UserHelpers.CreateToken(user, _tokensecret),
                User = user
            };
        }
    }
}
