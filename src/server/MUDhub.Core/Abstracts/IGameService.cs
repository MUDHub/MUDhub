using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IGameService
    {
        Task StartMudAsync(string mudId);
        Task StopMudAsync(string mudId);
    }
}
