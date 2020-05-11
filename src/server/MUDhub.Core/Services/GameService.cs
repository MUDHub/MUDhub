using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class GameService : IGameService
    {
        private readonly MudDbContext _context;

        public GameService(MudDbContext context)
        {
            _context = context;
        }

        public async Task<bool> StartMudAsync(string mudId, string userid)
        {
            var user = await _context.GetUserByIdAsnyc(userid)
                                         .ConfigureAwait(false);
            if (user is null)
            {
                return false;
            }
            var mud = user.MudGames.FirstOrDefault(mg => mg.Id == mudId);
            if (mud.State != MudGameState.InActive)
                return false;
            mud.State = MudGameState.Active;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> StopMudAsync(string mudId, string userid)
        {
            var user = await _context.GetUserByIdAsnyc(userid)
                                         .ConfigureAwait(false);
            if (user is null)
                return false;


            var mud = user.MudGames.FirstOrDefault(mg => mg.Id == mudId);
            if (mud is null)
                return false;
            if (mud.State != MudGameState.Active)
                return false;

            mud.State = MudGameState.InActive;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}
