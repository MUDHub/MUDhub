﻿using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Configurations;
using MUDhub.Core.Helper;
using MUDhub.Core.Services.Models;

namespace MUDhub.Core.Services
{
    internal class LoginService : ILoginService
    {
        private readonly MudDbContext _dbContext;
        private readonly IUserManager _userManager;
        //Todo: Moris => Brauchen wir UserSettings?
        private ServerConfiguration _userSettings;
        private readonly ILogger? _logger;
        public LoginService(MudDbContext context, IUserManager userManager, IOptions<ServerConfiguration> options, ILogger<LoginService>? logger = null)
            : this(context, userManager, options?.Value ?? throw new ArgumentNullException(nameof(options)), logger)
        {
        }

        internal LoginService(MudDbContext context, IUserManager userManager, ServerConfiguration options, ILogger<LoginService>? logger = null)
        {
            _dbContext = context;
            _userManager = userManager;
            _userSettings = options;
            _logger = logger;
        }

        private string CreateToken(User user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_userSettings.TokenSecret);
            var listClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name + " " + user.Lastname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (_userManager.IsUserInRole(user, (Roles)role))
                {
                    listClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listClaims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResult> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);
            var passwordHash = UserHelpers.CreatePasswordHash(password);
            if (user == null)
            {
                return new LoginResult(false);
            }

            if (passwordHash != user.PasswordHash)
            {
                
                return new LoginResult(false);
            }
            return new LoginResult(true, CreateToken(user));
        }
    }
}
