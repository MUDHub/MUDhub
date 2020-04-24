using MUDhub.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MUDhub.Core.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MUDhub.Core.Services
{ 
    internal class UserManager : IUserManager
    {


        private readonly MudDbContext _context;
        private readonly ILogger _logger;

        
        public UserManager(MudDbContext context, ILogger<UserManager>? logger = null)
        {
            //Todo: add Mailservice
            _context = context;
            _logger = logger;
        }


        /*public RegisterResult RegisterUser(RegistrationArgs model)
        {
            throw new NotImplementedException();
        }
        */
        public async Task<bool> RemoveUserAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);

            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            _logger?.LogInformation($"User: {user.Name} {user.Lastname}, removed.");
            return true;
        }

        public async Task<bool> AddRoleToUserAsync(string userId, Roles role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);
            if (user == null)
            {
                //Todo: add logmessage
                return false;
            }
            if ((user.Role & role) == role)
            {
                //Todo: add logmessage
                return false;
            }
            user.Role |= role;
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(string userId, Roles role)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            if ((user.Role & role) != role) return false;
            user.Role &= ~role;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> IsUserInRoleAsync(string userId, Roles role)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            return ((user.Role & role) == role);
        }

        public Task GeneratePasswortResetAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePasswortFromResetAsync(string passwordresetkey, string newPassword)
        {
            
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePasswortAsync(string userId, string oldPassword, string newPassword)
        {
            //Todo: async and logmessages
            var user = _context.Users.FirstOrDefaultAsync(u => u.PasswortHash == CreatePasswordHash(oldPassword)).Result;
            user.PasswortHash = CreatePasswordHash(newPassword);
            _context.Users.Update(user);
            _context.SaveChanges();
            throw new NotImplementedException();
        }

        private string CreatePasswordHash(string password)
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
    }
}
