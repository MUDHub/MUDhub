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
            if (user == null)
            {
                _logger?.LogWarning($"No user was found with the email {email}");
                return new LoginResult(false);
            }

            if (passwordHash != user.PasswordHash)
            {
                _logger?.LogWarning($"The Password of '{user.Name} {user.Lastname}' is wrong");
                return new LoginResult(false);
            }
            _logger?.LogInformation($"The User '{user.Name} {user.Lastname}'  was logged in.");
            return new LoginResult(true, UserHelpers.CreateToken(user, _tokensecret), user);
        }
    }
}
