using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Models;
using MUDhub.Core.Abstracts.Models;

namespace MUDhub.Core.Abstracts
{
    public interface IUserManager
    {
        Task<RegisterResult> RegisterUserAsync(UserModelArgs model);
        Task<bool> UpdateUserAsync(string userId, UserModelArgs model);
        Task<bool> RemoveUserAsync(string userId);
        Task<bool> AddRoleToUserAsync(string userId, Roles role);
        Task<bool> RemoveRoleFromUserAsync(string userId, Roles role);
        Task<bool> GeneratePasswortResetAsync(string email);
        Task<bool> UpdatePasswortFromResetAsync(string email, string newPassword);
        Task<bool> UpdatePasswortAsync(string userId, string oldPassword, string newPassword);
        Task<bool> IsUserInRoleAsync(string userId, Roles role);
        Task<User> GetUserByIdAsync(string userId);

    }
}
