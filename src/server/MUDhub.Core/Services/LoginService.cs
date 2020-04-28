using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Configurations;
using MUDhub.Core.Services.Models;

namespace MUDhub.Core.Services
{
    internal class LoginService : ILoginService
    {
        private readonly MudDbContext _dbContext;
        private readonly IUserManager _userManager;
        private readonly string _tokensecret;
        private readonly ILogger? _logger;
        public LoginService(MudDbContext context, IUserManager userManager, IOptions<ServerConfiguration> options)
            :this(context,userManager,options?.Value ?? throw new ArgumentNullException(nameof(options)))
        {
        }

        internal LoginService(MudDbContext context, IUserManager userManager, ServerConfiguration options)
        {
            _dbContext = context;
            _userManager = userManager;
            _tokensecret = options.TokenSecret;
        }

        public LoginResult LoginAsync()
        {
            throw new NotImplementedException();
        }

        private string CreateToken(string userId)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokensecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Email, userId),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.Role, "MUD Master"),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
