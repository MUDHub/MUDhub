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

        private const int DefaultHealth = 80;
        private readonly MudDbContext _context;
        private readonly ILogger<CharacterManager>? _logger;

        public CharacterManager(MudDbContext context, ILogger<CharacterManager>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CharacterResult> CreateCharacterAsync(string userid, string mudid, CharacterArgs args)
        {
            _logger?.LogInformation($"Userid:'{userid}' requested create a new character in mudgame '{mudid}'.");
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

            //Check if user has rights
            var isOwner = user.MudGames.FirstOrDefault(mg => mg.Id == mudid) != null;
            var canJoin = user.Joins.FirstOrDefault(j => j.MudId == mudid && j.State == MudJoinState.Accepted) != null;
            if (isOwner || canJoin)
            {
                //Check class id
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
                    var errormessage = $"Class with the id: '{args.ClassId}' exists but the mudid is not the rigth expected: '{mudid}' actual: '{characterclass.Game.Id}'.";
                    _logger?.LogInformation(errormessage);
                    return new CharacterResult()
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }

                //check race id
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
                    var errormessage = $"Class with the id: '{args.RaceId}' exists but the mudid is not the right expected: '{mudid}' actual: '{characterrace.Game.Id}'.";
                    _logger?.LogInformation(errormessage);
                    return new CharacterResult()
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }

                var character = new Character
                {
                    Owner = user,
                    Game = characterclass.Game,
                    Class = characterclass,
                    Race = characterrace,
                    Name = args.Name,
                    Health = DefaultHealth
                };
                _context.Characters.Add(character);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger?.LogInformation($"Successfully '{user.Email}' create character with the id '{character.Id}' and name '{character.Name}' in the mudgame '{character.Game.Name}'.");
                return new CharacterResult
                {
                    Character = character,
                    Success = true
                };
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

        public async Task<CharacterClassResult> CreateClassAsync(string userid, string mudid, CharacterClassArgs args)
        {
            _logger?.LogInformation($"Userid:'{userid}' requested create a new class in mudgame '{mudid}'.");
            var user = await _context.GetUserByIdAsnyc(userid)
                                        .ConfigureAwait(false);
            if (user is null)
            {
                var errormessage = $"User with the Userid: '{userid}' does not exist";
                _logger?.LogWarning(errormessage);
                return new CharacterClassResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }

            var isOwner = user.MudGames.FirstOrDefault(mg => mg.Id == mudid) != null;
            if (isOwner)
            {
                var mud = await _context.GetMudByIdAsnyc(mudid).ConfigureAwait(false);
                if (mud is null)
                {
                    var errormessage = $"Mud with the id '{mudid}' does not exist.";
                    _logger?.LogWarning(errormessage);
                    return new CharacterClassResult
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }
                else
                {
                    var characterclass = new CharacterClass
                    {
                        Game = mud,
                        Description = args.Desctiption,
                        Name = args.Name
                    };
                    _context.Classes.Add(characterclass);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    _logger?.LogInformation($"Successfully created new CharacterClass '{characterclass.Name}' with id: '{characterclass.Id}' in mudgame '{mudid}'");
                    return new CharacterClassResult
                    {
                        Class = characterclass,
                        Success = true
                    };
                }
            }
            else
            {
                var errormessage = $"User with the Userid: '{userid}' is not the owner from the mudgame '{mudid}' and can't create a new class.";
                _logger?.LogWarning(errormessage);
                return new CharacterClassResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }
        }

        public async Task<CharacterRaceResult> CreateRaceAsync(string userid, string mudid, CharacterRaceArgs args)
        {

            _logger?.LogInformation($"Userid:'{userid}' requested create a new race in mudgame '{mudid}'.");
            var user = await _context.GetUserByIdAsnyc(userid)
                                        .ConfigureAwait(false);
            if (user is null)
            {
                var errormessage = $"User with the Userid: '{userid}' does not exist";
                _logger?.LogWarning(errormessage);
                return new CharacterRaceResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }

            var isOwner = user.MudGames.FirstOrDefault(mg => mg.Id == mudid) != null;
            if (isOwner)
            {
                var mud = await _context.GetMudByIdAsnyc(mudid).ConfigureAwait(false);
                if (mud is null)
                {
                    var errormessage = $"Mud with the id '{mudid}' does not exist.";
                    _logger?.LogWarning(errormessage);
                    return new CharacterRaceResult
                    {
                        Success = false,
                        Errormessage = errormessage
                    };
                }
                else
                {
                    var characterRace = new CharacterRace
                    {
                        Game = mud,
                        Description = args.Desctiption,
                        Name = args.Name
                    };
                    _context.Races.Add(characterRace);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    _logger?.LogInformation($"Successfully created new CharacterClass '{characterRace.Name}' with id: '{characterRace.Id}' in mudgame '{mudid}'");
                    return new CharacterRaceResult
                    {
                        Race = characterRace,
                        Success = true
                    };
                }
            }
            else
            {
                var errormessage = $"User with the Userid: '{userid}' is not the owner from the mudgame '{mudid}' and can't create a new class.";
                _logger?.LogWarning(errormessage);
                return new CharacterRaceResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }
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
