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

        public static IEnumerable<string> ConvertRoleToList(Role role) 
            => Enum.GetValues(typeof(Role))
                       .Cast<Role>()
                       .Where(r => (r & role) != 0)
                       .Select(r => r.ToString());
    }
}
