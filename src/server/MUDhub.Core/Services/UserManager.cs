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
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor for the UserManager.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="emailService"></param>
        /// <param name="logger"></param>
        public UserManager(MudDbContext context, IEmailService emailService, ILogger<UserManager>? logger = null)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        /// <summary>
        /// A user is registered asynchronously.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                return new RegisterResult(false, true);
            }
            
        }

        /// <summary>
        /// One user is removed asynchronously.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// A role is added to the user asynchronously.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
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

        /// <summary>
        /// A role is asynchronously removed from the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks asynchronously if the user has the role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> IsUserInRoleAsync(string userId, Roles role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if (user is null)
            {
                return false;
            }
            return ((user.Role & role) == role);
        }

        /// <summary>
        /// Sends an asynchronous email with the user's ResetKey.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> GeneratePasswortResetAsync(string email)
        {
            return await _emailService.SendAsync(email, string.Empty).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously adds a new password to the user using the ResetKey.
        /// </summary>
        /// <param name="passwordresetkey"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Task<bool> UpdatePasswortFromResetAsync(string passwordresetkey, string newPassword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overwrites the old password of the user asynchronously with a new one.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
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

        /// <summary>
        /// A password hash is generated from the password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the user asynchronously using the UserID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
        }
    }
}
