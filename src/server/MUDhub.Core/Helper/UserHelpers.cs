using Microsoft.IdentityModel.Tokens;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MUDhub.Core.Helper
{
    public class UserHelpers
    {

        public static string CreatePasswordHash(string password)
        {
            byte[] data;
            using (HashAlgorithm algorithm = SHA256.Create())
                data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

            string passwordHash = string.Create(data.Length, data, (target, arg) =>
            {
                for (var i = 0; i < arg.Length; i += 2)
                {
                    var t = arg[i].ToString("X2", CultureInfo.InvariantCulture);
                    target[i] = Convert.ToChar(t[0]);
                    target[i + 1] = Convert.ToChar(t[1]);
                }
            });
            return passwordHash;
        }

        public static bool IsUserInRole(User user, Roles role)
        {
            if (user is null)
            {
                return false;
            }
            return ((user.Role & role) == role);
        }

        public static string CreateToken(User user, string tokensecret)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokensecret);
            var listClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name + " " + user.Lastname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (UserHelpers.IsUserInRole(user, (Roles)role))
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

        public static IEnumerable<string> ConvertRoleToList(Roles role) 
            => Enum.GetValues(typeof(Roles))
                       .Cast<Roles>()
                       .Where(r => (r & role) != 0)
                       .Select(r => r.ToString());
    }
}
