using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IGameService
    {
        Task<bool> StartMudAsync(string mudId, string userid);
        Task<bool> StopMudAsync(string mudId, string userid);

    }
}
