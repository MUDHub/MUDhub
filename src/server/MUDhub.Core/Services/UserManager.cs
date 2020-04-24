using MUDhub.Core.Abstracts;
using System;
using System.Globalization;
using System.Text;
using MUDhub.Core.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Services.Models;
using System.Net.Mail;

namespace MUDhub.Core.Services
{
    internal class UserManager : IUserManager
    {


        private readonly MudDbContext _context;
        private readonly ILogger? _logger;

        
        public UserManager(MudDbContext context, ILogger<UserManager>? logger = null)
        {
            //Todo: add Mailservice
            _context = context;
            _logger = logger;
        }


        public async Task<RegisterResult> RegisterUserAsync(RegistrationArgs model)
        {
            if (string.IsNullOrEmpty(model.Name) || 
                string.IsNullOrEmpty(model.Lastname) ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password))
            {
                return new RegisterResult(false);
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == model.Name && u.Lastname == model.Lastname)
                .ConfigureAwait(false);
            if(user == null)
            {
                var newUser = new User
                {
                    Name = model.Name,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    PasswordHash = CreatePasswordHash(model.Password)
                };
                await _context.AddAsync(newUser);
                await _context.SaveChangesAsync()
                    .ConfigureAwait(false);
                return new RegisterResult(true);
            }
            else
            {
                return new RegisterResult(false);
            }
            
        }
        
        public async Task<bool> RemoveUserAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);

            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            _logger?.LogInformation($"User: '{user.Name} {user.Lastname}' is removed.");
            return true;
        }

        public async Task<bool> AddRoleToUserAsync(string userId, Roles role)
        {
            var user = await GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                _logger?.LogWarning($"The role: '{role}' could not be added to user: '{user?.Name} {user?.Lastname}'. => User does not exist");
                return false;
            }
            if ((user.Role & role) == role)
            {
                _logger?.LogWarning($"The role: '{role}' could not be added to user: '{user.Name} {user.Lastname}'. => User already has the role");
                return false;
            }
            user.Role |= role;
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The role: '{role}' was added to user: '{user.Name} {user.Lastname}'.");
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(string userId, Roles role)
        {
            var user = await GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                _logger?.LogWarning($"The role: '{role}' could not be removed from user: '{user?.Name} {user?.Lastname}'. => User does not exist");
                return false;
            }
            if ((user.Role & role) != role)
            {
                _logger?.LogWarning($"The role: '{role}' could not be removed from user: '{user.Name} {user.Lastname}'. => User has not the role");
                return false;
            }
            user.Role &= ~role;
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The role: '{role}' was removed from user: '{user.Name} {user.Lastname}'.");
            return true;
        }
        public async Task<bool> IsUserInRoleAsync(string userId, Roles role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if (user is null)
            {
                return false;
            }
            return ((user.Role & role) == role);
        }

        public bool GeneratePasswortResetAsync(string email)
        {
            var mailService = new EmailService();
            var mailMaker = new MailMaker()
            {
                Sender = "??@??.de",
                Receiver = email,
                Subject = "Registrierung bei MUDhub",
                Message = "Drücke hier ink",
                Servername = "????",
                Port = "22",
                Username = "???",
                Password = "????"
            };
            return mailService.Send(mailMaker);
        }

        public Task<bool> UpdatePasswortFromResetAsync(string passwordresetkey, string newPassword)
        {
            
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePasswortAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                _logger?.LogWarning($"The Password of '{user?.Name} {user?.Lastname}' could not be updated. => User does not exist.");
                return false;
            }
            if (oldPassword == newPassword)
            {
                _logger?.LogWarning($"The Password of '{user.Name} {user.Lastname}' could not be updated. => Old password same as new password.");
                return false;
            }
            user.PasswordHash = CreatePasswordHash(newPassword);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            _logger?.LogInformation($"The Password of '{user.Name} {user.Lastname}' was updated.");
            return true;
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
        private async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
        }
    }
}
