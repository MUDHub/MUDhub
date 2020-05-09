using MUDhub.Core.Abstracts;
using System;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class GameService : IGameService
    {
        public Task<bool> StartMudAsync(string mudId, string userid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopMudAsync(string mudId, string userid)
        {
            throw new NotImplementedException();
        }
    }
}
