using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class CharacterManager : ICharacterManager
    {
        private readonly MudDbContext _context;
        private readonly ILogger<CharacterManager>? _logger;

        public CharacterManager(MudDbContext context, ILogger<CharacterManager>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CharacterResult> CreateCharacterAsync(string userid, string mudid, CharacterArgs args)
        {
            var user = await _context.GetUserByIdAsnyc(userid)
                                        .ConfigureAwait(false);
            if (user is null)
            {
                var errormessage = $"User with the Userid: '{userid}' does not exist";
                _logger?.LogWarning(errormessage);
                return new CharacterResult
                {
                    Errormessage = errormessage,
                    Success = false
                };
            }
            var isOwner = user.MudGames.FirstOrDefault(mg => mg.Id == mudid) != null;
            var canJoin = user.Joins.FirstOrDefault(j => j.MudId == mudid && j.State == MudJoinState.Accepted) != null;
            if (isOwner || canJoin)
            {
                throw new NotImplementedException();
            }
            else
            {
                var errorMessage = $"User '{user.Email}' has no rights to create a Character on the mudgame with the id'{mudid}'";
                _logger?.LogWarning(errorMessage);
                return new CharacterResult
                {
                    Success = false,
                    Errormessage = errorMessage
                };
            }
        }

        public Task<CharacterClassResult> CreateClassAsync(string userid, string mudid, CharacterClassArgs args)
        {

            return Task.FromResult(new CharacterClassResult());
        }

        public Task<CharacterRaceResult> CreateRaceAsync(string userid, string mudid, CharacterRaceArgs args)
        {

            return Task.FromResult(new CharacterRaceResult());
        }

        public Task<CharacterResult> RemoveCharacterAsync(string userid, string characterid)
        {

            return Task.FromResult(new CharacterResult());
        }

        public Task<CharacterClassResult> RemoveClassAsync(string userid, string classid)
        {

            return Task.FromResult(new CharacterClassResult());
        }

        public Task<CharacterRaceResult> RemoveRaceAsync(string userid, string raceid)
        {

            return Task.FromResult(new CharacterRaceResult());
        }
    }
}
