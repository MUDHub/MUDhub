using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IGameService
    {
        Task<bool> StartMudAsync(string mudId);
        Task<bool> StopMudAsync(string mudId);


    }
}
