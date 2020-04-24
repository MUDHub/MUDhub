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
        /*public RegisterResult RegisterUser(RegistrationArgs model)
        {
            throw new NotImplementedException();
        }
        */
        public bool RemoveUser(string userId)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            if (user == null) return false;
            _context.Users.Remove(user);
            return true;
        }

        public bool AddRoleToUser(string userId, Roles role)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            if ((user.Role & role) == role) return false;
            user.Role |= role;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public bool RemoveRoleFromUser(string userId, Roles role)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            if ((user.Role & role) != role) return false;
            user.Role &= ~role;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
        public bool IsUserInRole(string userId, Roles role)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            return ((user.Role & role) == role);
        }

        public void GeneratePasswortReset(string email)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePasswortFromReset(string email, string newPassword)
        {
            
            throw new NotImplementedException();
        }

        public bool UpdatePasswort(string userId, string oldPassword, string newPassword)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.PasswortHash == oldPassword).Result;
            user.PasswortHash = newPassword;
            _context.Users.Update(user);
            _context.SaveChanges();
            throw new NotImplementedException();
        }

        public string CreatePasswortHash(string password)
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
