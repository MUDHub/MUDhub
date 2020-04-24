using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models;

namespace MUDhub.Core.Abstracts
{
    public interface IUserManager
    {
        //RegisterResult RegisterUser(RegistrationArgs model);
        bool RemoveUser(string userId);
        bool AddRoleToUser(string userId, Roles role);
        bool RemoveRoleFromUser(string userId, Roles role);
        void GeneratePasswortReset(string email);
        bool UpdatePasswortFromReset(string email, string newPassword);
        bool UpdatePasswort(string userId, string oldPassword, string newPassword);
        bool IsUserInRole(string userId, Roles role);
        string CreatePasswortHash(string password);
    }
}
