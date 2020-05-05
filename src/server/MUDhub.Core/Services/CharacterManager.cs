using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts.Models.Characters;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models;

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
            _logger?.LogInformation($"Userid:'{userid}' requested create a new character");
            var user = await _context.GetUserByIdAsnyc(userid)
                                        .ConfigureAwait(false);
            if (user is null)
            {
                var errormessage = $"User with the Userid: '{userid}' does not exist";
                _logger?.LogWarning(errormessage);
                return new CharacterResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }



            var isOwner = user.MudGames.FirstOrDefault(mg => mg.Id == mudid) != null;
            var canJoin = user.Joins.FirstOrDefault(j => j.MudId == mudid && j.State == MudJoinState.Accepted) != null;
            if (isOwner || canJoin)
            {
               var characterclass = await GetClassByIdAsync(args.ClassId)
                                                .ConfigureAwait(false);
                if (characterclass is null)
                {
                    var errormessage = $"Class with the id: '{args.ClassId}' does not exist.";
                    _logger?.LogInformation(errormessage);
                    return new CharacterResult()
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }
                if (characterclass.Game.Id != mudid)
                {
                    var errormessage = $"Class with the id: '{args.ClassId}' exists but the mudid is not equals expected: '{mudid}' actual: '{characterclass.Game.Id}'.";
                    _logger?.LogInformation(errormessage);
                    return new CharacterResult()
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }
                var characterrace = await GetRaceByIdAsync(args.RaceId)
                                                .ConfigureAwait(false);
                if (characterrace is null)
                {
                    var errormessage = $"Race with the id: '{args.RaceId}' does not exist.";
                    return new CharacterResult()
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }
                if (characterrace.Game.Id != mudid)
                {
                    var errormessage = $"Class with the id: '{args.RaceId}' exists but the mudid is not equals expected: '{mudid}' actual: '{characterrace.Game.Id}'.";
                    _logger?.LogInformation(errormessage);
                    return new CharacterResult()
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }


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
            throw new NotImplementedException();
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



        private async Task<CharacterClass?> GetClassByIdAsync(string id) 
            => await _context.Classes.FindAsync(id)
                                     .ConfigureAwait(false);
        private async Task<CharacterRace?> GetRaceByIdAsync(string id)
           => await _context.Races.FindAsync(id)
                                    .ConfigureAwait(false);
    }
}
