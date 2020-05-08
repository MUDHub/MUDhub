using MUDhub.Core.Abstracts;
using System;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class GameService : IGameService
    {
        public Task<bool> StartMudAsync(string mudId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopMudAsync(string mudId)
        {
            throw new NotImplementedException();
        }
    }
}
