using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IMudManager
    {
        Task<MudGame?> CreateMudAsync(string name, MudCreationArgs args);
        Task<bool> RemoveMudAsync(string mudId);
        Task<MudGame?> UpdateMudAsync(string mudid, MudUpdateArgs args);

        Task<bool> SetEditModeAsync(string mudId, string userid, bool isInEdit);

        Task<bool> RequestUserForJoinAsync(string userId, string mudId);
        Task<bool> ApproveUserToJoinAsync(string userId, string mudId);
        Task<bool> RejectUserToJoinAsync(string userId, string mudId);
    }
}
