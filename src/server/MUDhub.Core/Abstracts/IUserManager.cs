using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models.Users;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IUserManager
    {
        Task<RegisterResult> RegisterUserAsync(RegistrationUserArgs model);
        Task<User?> UpdateUserAsync(string userId, UpdateUserArgs model);
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
