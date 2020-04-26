using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IMudManager
    {
        Task<(bool Success, string MudId)> CreateMudAsync(string name, MudCreationArgs args);
        Task<bool> RemoveMudAsync(string mudId);

        Task<bool> UpdateMudAsync(string mudid, MudUpdateArgs args);

        Task<bool> RequestUserForJoinAsync(string userId, string mudId);
        Task<bool> ApproveUserToJoinAsync(string userId, string mudId);
        Task<bool> RejectUserToJoinAsync(string userId, string mudId);
    }
}
