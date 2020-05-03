﻿using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using System.Threading.Tasks;

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
        public async Task<RegisterResult> RegisterUserAsync(RegistrationUserArgs model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) ||
                model.Lastname is null ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return new RegisterResult(false);
            }
            var normalizedEmail = model.Email.ToUpperInvariant();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail)
                .ConfigureAwait(false);
            if (user == null)
            {
                var newUser = new User
                {
                    Name = model.Name,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpperInvariant(),
                    PasswordHash = UserHelpers.CreatePasswordHash(model.Password)
                };
                await _context.AddAsync(newUser).ConfigureAwait(false);
                await _context.SaveChangesAsync()
                    .ConfigureAwait(false);
                return new RegisterResult(true, user: newUser);
            }

            return new RegisterResult(false, true);

        }


        public async Task<bool> UpdateUserAsync(string userId, UpdateUserArgs model)
        {
            var user = await GetUserByIdAsync(userId)
                .ConfigureAwait(false);
            if (user is null)
            {
                _logger?.LogWarning($"UserID: '{userId}' didn't exists. No Update possible.");
                return false;
            }
            if (model.Name != null)
                user.Name = model.Name;
            if (model.Lastname != null)
                user.Lastname = model.Lastname;


            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"User with id: '{userId}', was successfully updated, with the given arguments: {Environment.NewLine}" +
                $"- Name: {model.Name ?? "<no modification>"},{Environment.NewLine}" +
                $"- Description: {model.Lastname ?? "<no modification>"},{Environment.NewLine}");
            return true;
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

            if (user == null)
            {
                _logger?.LogWarning($"Could not remove user: '{userId}'. => User does not exist");
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);

            _logger?.LogInformation($"User: '{user.Email}' is removed.");
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
                _logger?.LogWarning($"The role: '{role}' could not be added to user: '{userId}'. => User does not exist");
                return false;
            }
            if ((user.Role & role) == role)
            {
                _logger?.LogWarning($"The role: '{role}' could not be added to user: '{user.Email}'. => User already has the role");
                return false;
            }
            user.Role |= role;
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The role: '{role}' was added to user: '{user.Email}'.");
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
                _logger?.LogWarning($"The role: '{role}' could not be removed from user: '{userId}'. => User does not exist");
                return false;
            }
            if ((user.Role & role) != role)
            {
                _logger?.LogWarning($"The role: '{role}' could not be removed from user: '{user.Email}'. => User has not the role");
                return false;
            }
            user.Role &= ~role;
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The role: '{role}' was removed from user: '{user.Email}'.");
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
            return UserHelpers.IsUserInRole(user, role);
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
        public async Task<bool> UpdatePasswortFromResetAsync(string passwordresetkey, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetKey == passwordresetkey)
                .ConfigureAwait(false);
            if (user == null)
            {
                _logger?.LogWarning($"The Password from resetkey '{passwordresetkey}' could not be reseted. => Related User does not exist, key does not exists.");
                return false;
            }

            var newPasswordHash = UserHelpers.CreatePasswordHash(newPassword);

            if (user.PasswordHash == newPasswordHash)
            {
                _logger?.LogWarning($"The Password of '{user.Email}' could not be reseted. => Old password same as new password.");
                return false;
            }

            user.PasswordHash = newPasswordHash;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            _logger?.LogInformation($"The Password of '{user.Email}' was reseted.");
            return true;
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
                _logger?.LogWarning($"The Password of '{userId}' could not be updated. => User does not exist.");
                return false;
            }
            if (oldPassword == newPassword)
            {
                _logger?.LogWarning($"The Password of '{user.Email}' could not be updated. => Old password same as new password.");
                return false;
            }
            if (UserHelpers.CreatePasswordHash(oldPassword) != user.PasswordHash)
            {
                _logger?.LogWarning($"The Password of '{user.Email}' could not be updated. => Old password is not the actual password.");
                return false;
            }

            user.PasswordHash = UserHelpers.CreatePasswordHash(newPassword);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            _logger?.LogInformation($"The Password of '{user.Email}' was updated.");
            return true;
        }


        /// <summary>
        /// Get the user asynchronously using the UserID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
        }
    }
}
