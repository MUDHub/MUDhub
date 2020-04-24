using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IMudManager
    {
        Task<MudGame> CreateMudAsync(string name, string description, string imagekey, bool isPublic = true);
        Task<bool> RemoveMudAsync(string mudId);

        Task<bool> RequestUserForJoinAsync(string userId, string mudId);
        Task<bool> AcceptUserToJoinAsync(string userId, string mudId);
    }
}
