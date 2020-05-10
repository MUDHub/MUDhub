using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Characters;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Muds;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// A new character is created.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="mudid"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
                    Health = DefaultHealth,
                    Inventory = new Inventory()
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

        /// <summary>
        /// A new class is created.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="mudid"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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

        /// <summary>
        /// A new race is created.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="mudid"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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

            var mud = user.MudGames.FirstOrDefault(mg => mg.Id == mudid);
            if (mud != null)
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

        /// <summary>
        /// A class is updated.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="classId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<CharacterClassResult> UpdateClassAsync(string userid, string classId, CharacterClassArgs args)
        {
            _logger?.LogInformation($"Userid:'{userid}' requested update a class in mudgame '{classId}'.");
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

            var mudgame = user.MudGames.FirstOrDefault(mg => mg.Classes.Any(c => c.Id == classId));
            if (mudgame is null)
            {
                var errormessage = $"Class with the id '{classId}' user is not the owner.";
                _logger?.LogWarning(errormessage);
                return new CharacterClassResult
                {
                    Success = false,
                    Errormessage = errormessage
                };

            }
            else
            {
                var characterclass = mudgame.Classes.FirstOrDefault(c => c.Id == classId);
                characterclass.Description = args.Desctiption;
                characterclass.Name = args.Name;
                characterclass.ImageKey = args.ImageKey;
                _context.Classes.Update(characterclass);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger?.LogInformation($"Successfully updated new CharacterClass '{characterclass.Name}' with id: '{characterclass.Id}' in mudgame '{mudgame.Name}'");
                return new CharacterClassResult
                {
                    Class = characterclass,
                    Success = true
                };
            }
        }

        /// <summary>
        /// A race is updated.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="raceId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<CharacterRaceResult> UpdateRaceAsync(string userid, string raceId, CharacterRaceArgs args)
        {

            _logger?.LogInformation($"Userid:'{userid}' requested create a new class in mudgame '{raceId}'.");
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

            var mudgame = user.MudGames.FirstOrDefault(mg => mg.Races.Any(c => c.Id == raceId));
            if (mudgame is null)
            {
                var errormessage = $"Class with the id '{raceId}' user is not the owner.";
                _logger?.LogWarning(errormessage);
                return new CharacterRaceResult
                {
                    Success = false,
                    Errormessage = errormessage
                };

            }
            else
            {
                var characterrace = mudgame.Races.FirstOrDefault(c => c.Id == raceId);
                characterrace.Description = args.Desctiption;
                characterrace.Name = args.Name;
                characterrace.ImageKey = args.ImageKey;
                _context.Races.Update(characterrace);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger?.LogInformation($"Successfully updated new CharacterRace '{characterrace.Name}' with id: '{characterrace.Id}' in mudgame '{mudgame.Name}'");
                return new CharacterRaceResult
                {
                    Race = characterrace,
                    Success = true
                };
            }
        }

        /// <summary>
        /// A character is removed.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="characterid"></param>
        /// <returns></returns>
        public async Task<CharacterResult> RemoveCharacterAsync(string userid, string characterid)
        {
            _logger?.LogInformation($"Userid:'{userid}' requested remove a old character with id '{characterid}'.");
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

            var character = user.Characters.FirstOrDefault(c => c.Id == characterid);
            if (character is null)
            {
                var errormessage = $"User '{user.Email}' is not the owner from Character with the id: '{characterid}'";
                _logger?.LogWarning(errormessage);
                return new CharacterResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync()
                            .ConfigureAwait(false);
            return new CharacterResult()
            {
                Character = character,
                Success = true
            };
        }

        /// <summary>
        /// A class is removed.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public async Task<CharacterClassResult> RemoveClassAsync(string userid, string classid)
        {

            _logger?.LogInformation($"Userid:'{userid}' requested remove a old character with id '{classid}'.");
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

            var mudgame = user.MudGames.FirstOrDefault(m => m.Classes.Any(c => c.Id == classid));
            var characterClass = mudgame?.Classes.FirstOrDefault(c => c.Id == classid);
            if (characterClass is null)
            {
                var errormessage = $"User '{user.Email}' is not the owner from CharacterClass with the id: '{classid}'";
                _logger?.LogWarning(errormessage);
                return new CharacterClassResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }

            _context.Classes.Remove(characterClass);
            await _context.SaveChangesAsync()
                            .ConfigureAwait(false);
            _logger?.LogInformation($"Successfully removed new characterclass '{characterClass.Name}' with id: '{characterClass.Id}' in mudgame '{mudgame.Name}'");

            return new CharacterClassResult()
            {
                Class = characterClass,
                Success = true
            };
        }

        /// <summary>
        /// A race is removed.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="raceid"></param>
        /// <returns></returns>
        public async Task<CharacterRaceResult> RemoveRaceAsync(string userid, string raceid)
        {

            _logger?.LogInformation($"Userid:'{userid}' requested remove a old character with id '{raceid}'.");
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

            var mudgame = user.MudGames.FirstOrDefault(m => m.Races.Any(c => c.Id == raceid));
            var characterRace = mudgame?.Races.FirstOrDefault(c => c.Id == raceid);
            if (characterRace is null)
            {
                var errormessage = $"User '{user.Email}' is not the owner from CharacterClass with the id: '{raceid}'";
                _logger?.LogWarning(errormessage);
                return new CharacterRaceResult
                {
                    Success = false,
                    Errormessage = errormessage
                };
            }

            _context.Races.Remove(characterRace);
            await _context.SaveChangesAsync()
                            .ConfigureAwait(false);
            _logger?.LogInformation($"Successfully removed new CharacterClass '{characterRace.Name}' with id: '{characterRace.Id}' in mudgame '{mudgame.Name}'");
            return new CharacterRaceResult()
            {
                Race = characterRace,
                Success = true
            };
        }

        private async Task<CharacterClass?> GetClassByIdAsync(string id)
            => await _context.Classes.FindAsync(id)
                                     .ConfigureAwait(false);
        private async Task<CharacterRace?> GetRaceByIdAsync(string id)
           => await _context.Races.FindAsync(id)
                                    .ConfigureAwait(false);
    }
}
